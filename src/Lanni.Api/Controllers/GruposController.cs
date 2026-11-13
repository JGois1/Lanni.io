using Lanni.Api.Contracts;
using Lanni.Domain.Entities;
using Lanni.Domain.Services;
using Lanni.Infrastructure;
using Lanni.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lanni.Api.Controllers;

[ApiController]
[Route("api/grupos")]
public class GruposController : ControllerBase
{
    private readonly LanniDbContext _db;

    public GruposController(LanniDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<GrupoResponse>> Criar(CriarGrupoRequest request, CancellationToken ct)
    {
        Grupo grupo;
        try
        {
            grupo = new Grupo(request.Nome);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }

        _db.Grupos.Add(grupo);
        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(nameof(ObterPorId), new { id = grupo.Id }, ParaResponse(grupo));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GrupoResponse>> ObterPorId(Guid id, CancellationToken ct)
    {
        var grupo = await _db.Grupos
            .AsNoTracking()
            .Include(g => g.Participantes)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (grupo is null)
            return NotFound();

        return ParaResponse(grupo);
    }

    [HttpPost("{id:guid}/participantes")]
    public async Task<ActionResult<ParticipanteResponse>> AdicionarParticipante(
        Guid id, AdicionarParticipanteRequest request, CancellationToken ct)
    {
        var grupo = await _db.Grupos
            .Include(g => g.Participantes)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (grupo is null)
            return NotFound();

        Participante participante;
        try
        {
            participante = grupo.AdicionarParticipante(request.Nome);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }

        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = grupo.Id },
            new ParticipanteResponse(participante.Id, participante.Nome));
    }

    [HttpPost("{id:guid}/despesas")]
    public async Task<ActionResult<DespesaResponse>> AdicionarDespesa(
        Guid id, AdicionarDespesaRequest request, CancellationToken ct)
    {
        var grupo = await _db.Grupos
            .Include(g => g.Participantes)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (grupo is null)
            return NotFound();

        Despesa despesa;
        try
        {
            despesa = grupo.AdicionarDespesa(request.PagadorId, request.Descricao, request.Valor, request.ParticipanteIds);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }

        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = grupo.Id },
            new DespesaResponse(
                despesa.Id,
                despesa.PagadorId,
                despesa.Descricao,
                despesa.Valor,
                despesa.Data,
                despesa.Divisoes.Select(d => new DivisaoResponse(d.ParticipanteId, d.Valor)).ToList()));
    }

    [HttpPost("{id:guid}/acertos")]
    public async Task<ActionResult<AcertoResponse>> RegistrarAcerto(
        Guid id, RegistrarAcertoRequest request, CancellationToken ct)
    {
        var grupo = await _db.Grupos
            .Include(g => g.Participantes)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

        if (grupo is null)
            return NotFound();

        Acerto acerto;
        try
        {
            acerto = grupo.RegistrarAcerto(request.DeId, request.ParaId, request.Valor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { erro = ex.Message });
        }

        await _db.SaveChangesAsync(ct);

        return CreatedAtAction(
            nameof(ObterPorId),
            new { id = grupo.Id },
            new AcertoResponse(acerto.Id, acerto.DeId, acerto.ParaId, acerto.Valor, acerto.Data));
    }

    [HttpGet("{id:guid}/saldos")]
    public async Task<ActionResult<IReadOnlyList<SaldoParticipante>>> ObterSaldos(Guid id, CancellationToken ct)
    {
        if (!await _db.Grupos.AnyAsync(g => g.Id == id, ct))
            return NotFound();

        return Ok(await _db.ObterSaldosAsync(id, ct));
    }

    [HttpGet("{id:guid}/transferencias")]
    public async Task<ActionResult<IReadOnlyList<TransferenciaResponse>>> ObterTransferencias(Guid id, CancellationToken ct)
    {
        if (!await _db.Grupos.AnyAsync(g => g.Id == id, ct))
            return NotFound();

        var saldos = await _db.ObterSaldosAsync(id, ct);
        var nomes = saldos.ToDictionary(s => s.ParticipanteId, s => s.Nome);

        return Ok(SimplificadorDividas.Simplificar(saldos.Select(s => (s.ParticipanteId, s.Saldo)))
            .Select(t => new TransferenciaResponse(t.DeId, nomes[t.DeId], t.ParaId, nomes[t.ParaId], t.Valor))
            .ToList());
    }

    private static GrupoResponse ParaResponse(Grupo g) => new(
        g.Id,
        g.Nome,
        g.CriadoEm,
        g.Participantes.Select(p => new ParticipanteResponse(p.Id, p.Nome)).ToList());
}