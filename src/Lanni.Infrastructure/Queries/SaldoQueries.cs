using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Lanni.Infrastructure.Queries;

public record SaldoParticipante(Guid ParticipanteId, string Nome, decimal Saldo);

public static class SaldoQueries
{
    // Saldo > 0: o participante tem a receber; < 0: tem a pagar.
    private const string Sql = """
        SELECT p.Id AS ParticipanteId, p.Nome,
              COALESCE((SELECT SUM(d.Valor) FROM Despesas d WHERE d.PagadorId = p.Id), 0)
            - COALESCE((SELECT SUM(x.Valor) FROM DivisoesDespesa x WHERE x.ParticipanteId = p.Id), 0)
            + COALESCE((SELECT SUM(a.Valor) FROM Acertos a WHERE a.DeId = p.Id), 0)
            - COALESCE((SELECT SUM(a.Valor) FROM Acertos a WHERE a.ParaId = p.Id), 0) AS Saldo
        FROM Participantes p
        WHERE p.GrupoId = @grupoId
        ORDER BY p.Nome
        """;

    public static async Task<IReadOnlyList<SaldoParticipante>> ObterSaldosAsync(
        this LanniDbContext db, Guid grupoId, CancellationToken ct)
    {
        var saldos = await db.Database.GetDbConnection()
            .QueryAsync<SaldoParticipante>(new CommandDefinition(Sql, new { grupoId }, cancellationToken: ct));
        return saldos.AsList();
    }
}
