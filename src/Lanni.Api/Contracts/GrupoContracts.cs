namespace Lanni.Api.Contracts;

public record CriarGrupoRequest(string Nome);

public record AdicionarParticipanteRequest(string Nome);

public record ParticipanteResponse(Guid Id, string Nome);

public record AdicionarDespesaRequest(Guid PagadorId, string Descricao, decimal Valor, IReadOnlyList<Guid> ParticipanteIds);

public record RegistrarAcertoRequest(Guid DeId, Guid ParaId, decimal Valor);

public record AcertoResponse(Guid Id, Guid DeId, Guid ParaId, decimal Valor, DateTime Data);

public record DivisaoResponse(Guid ParticipanteId, decimal Valor);

public record DespesaResponse(
    Guid Id,
    Guid PagadorId,
    string Descricao,
    decimal Valor,
    DateTime Data,
    IReadOnlyList<DivisaoResponse> Divisoes);

public record GrupoResponse(
    Guid Id,
    string Nome,
    DateTime CriadoEm,
    IReadOnlyList<ParticipanteResponse> Participantes);