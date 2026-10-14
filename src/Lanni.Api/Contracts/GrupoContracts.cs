namespace Lanni.Api.Contracts;

public record CriarGrupoRequest(string Nome);

public record AdicionarParticipanteRequest(string Nome);

public record ParticipanteResponse(Guid Id, string Nome);

public record GrupoResponse(
    Guid Id,
    string Nome,
    DateTime CriadoEm,
    IReadOnlyList<ParticipanteResponse> Participantes);