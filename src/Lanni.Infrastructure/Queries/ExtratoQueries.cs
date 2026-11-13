using Dapper;
using Microsoft.EntityFrameworkCore;

namespace Lanni.Infrastructure.Queries;

// Despesa: De = quem pagou, Para = null. Acerto: De pagou a Para, Descricao = null.
public record MovimentoExtrato(string Tipo, Guid Id, DateTime Data, string? Descricao, decimal Valor, string DeNome, string? ParaNome);

public static class ExtratoQueries
{
    // shortcut: sem paginação, traz o grupo inteiro; paginar quando grupos tiverem centenas de movimentos.
    private const string Sql = """
        SELECT 'Despesa' AS Tipo, d.Id, d.Data, d.Descricao, d.Valor, p.Nome AS DeNome, NULL AS ParaNome
        FROM Despesas d
        JOIN Participantes p ON p.Id = d.PagadorId
        WHERE d.GrupoId = @grupoId
        UNION ALL
        SELECT 'Acerto', a.Id, a.Data, NULL, a.Valor, de.Nome, para.Nome
        FROM Acertos a
        JOIN Participantes de ON de.Id = a.DeId
        JOIN Participantes para ON para.Id = a.ParaId
        WHERE a.GrupoId = @grupoId
        ORDER BY Data DESC
        """;

    public static async Task<IReadOnlyList<MovimentoExtrato>> ObterExtratoAsync(
        this LanniDbContext db, Guid grupoId, CancellationToken ct)
    {
        var movimentos = await db.Database.GetDbConnection()
            .QueryAsync<MovimentoExtrato>(new CommandDefinition(Sql, new { grupoId }, cancellationToken: ct));
        return movimentos.AsList();
    }
}
