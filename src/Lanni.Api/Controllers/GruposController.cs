using Lanni.Api.Contracts;
using Lanni.Domain.Entities;
using Lanni.Infrastructure;
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

    private static GrupoResponse ParaResponse(Grupo g) => new(
        g.Id,
        g.Nome,
        g.CriadoEm,
        g.Participantes.Select(p => new ParticipanteResponse(p.Id, p.Nome)).ToList());
}