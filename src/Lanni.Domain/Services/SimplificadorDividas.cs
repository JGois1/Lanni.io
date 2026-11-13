namespace Lanni.Domain.Services;

public record Transferencia(Guid DeId, Guid ParaId, decimal Valor);

public static class SimplificadorDividas
{
    // shortcut: guloso (maior devedor paga ao maior credor); usa no máximo n-1 transferências,
    // mas nem sempre o mínimo absoluto (problema NP-difícil). Trocar só se grupos grandes exigirem.
    public static IReadOnlyList<Transferencia> Simplificar(IEnumerable<(Guid ParticipanteId, decimal Saldo)> saldos)
    {
        var lista = saldos.ToList();
        if (lista.Sum(s => s.Saldo) != 0)
            throw new InvalidOperationException("A soma dos saldos do grupo deve ser zero.");

        var devedores = lista.Where(s => s.Saldo < 0)
            .Select(s => (s.ParticipanteId, Valor: -s.Saldo)).OrderByDescending(s => s.Valor).ToList();
        var credores = lista.Where(s => s.Saldo > 0)
            .Select(s => (s.ParticipanteId, Valor: s.Saldo)).OrderByDescending(s => s.Valor).ToList();

        var transferencias = new List<Transferencia>();
        int i = 0, j = 0;
        while (i < devedores.Count && j < credores.Count)
        {
            var valor = Math.Min(devedores[i].Valor, credores[j].Valor);
            transferencias.Add(new Transferencia(devedores[i].ParticipanteId, credores[j].ParticipanteId, valor));

            devedores[i] = devedores[i] with { Valor = devedores[i].Valor - valor };
            credores[j] = credores[j] with { Valor = credores[j].Valor - valor };
            if (devedores[i].Valor == 0) i++;
            if (credores[j].Valor == 0) j++;
        }

        return transferencias;
    }
}
