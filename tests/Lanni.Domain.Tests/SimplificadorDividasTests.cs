using Lanni.Domain.Services;

namespace Lanni.Domain.Tests;

public class SimplificadorDividasTests
{
    private static readonly Guid Ana = Guid.NewGuid(), Bia = Guid.NewGuid(), Caio = Guid.NewGuid(), Davi = Guid.NewGuid();

    [Fact]
    public void Quem_deve_paga_a_quem_tem_a_receber()
    {
        var t = SimplificadorDividas.Simplificar([(Ana, 33.34m), (Bia, 0m), (Caio, -33.34m)]);

        Assert.Equal([new Transferencia(Caio, Ana, 33.34m)], t);
    }

    [Fact]
    public void Corrente_de_dividas_vira_uma_transferencia_direta()
    {
        // Ana devia 10 a Bia, que devia 10 a Caio: Bia fica zerada e Ana paga direto a Caio.
        var t = SimplificadorDividas.Simplificar([(Ana, -10m), (Bia, 0m), (Caio, 10m)]);

        Assert.Equal([new Transferencia(Ana, Caio, 10m)], t);
    }

    [Fact]
    public void Quita_todos_os_saldos_com_no_maximo_n_menos_1_transferencias()
    {
        (Guid, decimal)[] saldos = [(Ana, 50m), (Bia, 20.5m), (Caio, -45.25m), (Davi, -25.25m)];

        var t = SimplificadorDividas.Simplificar(saldos);

        Assert.True(t.Count <= saldos.Length - 1);
        Assert.All(t, x => Assert.True(x.Valor > 0));
        foreach (var (id, saldo) in saldos)
            Assert.Equal(saldo, t.Where(x => x.ParaId == id).Sum(x => x.Valor) - t.Where(x => x.DeId == id).Sum(x => x.Valor));
    }

    [Fact]
    public void Grupo_quitado_nao_gera_transferencias()
    {
        Assert.Empty(SimplificadorDividas.Simplificar([(Ana, 0m), (Bia, 0m)]));
        Assert.Empty(SimplificadorDividas.Simplificar([]));
    }

    [Fact]
    public void Rejeita_saldos_que_nao_somam_zero()
    {
        Assert.Throws<InvalidOperationException>(() => SimplificadorDividas.Simplificar([(Ana, 10m), (Bia, -5m)]));
    }
}
