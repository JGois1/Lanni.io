using Lanni.Domain.Entities;

namespace Lanni.Domain.Tests;

public class GrupoDespesaTests
{
    private static (Grupo grupo, Guid[] ids) GrupoCom(int participantes)
    {
        var grupo = new Grupo("Viagem");
        var ids = Enumerable.Range(1, participantes)
            .Select(i => grupo.AdicionarParticipante($"P{i}").Id)
            .ToArray();
        return (grupo, ids);
    }

    [Theory]
    [InlineData(100, 3, new[] { 33.33, 33.33, 33.34 })]
    [InlineData(90, 3, new[] { 30.0, 30.0, 30.0 })]
    [InlineData(0.01, 2, new[] { 0.0, 0.01 })]
    [InlineData(10, 1, new[] { 10.0 })]
    public void Divide_igualmente_e_o_centavo_que_sobra_fica_com_o_ultimo(decimal valor, int n, double[] esperado)
    {
        var (grupo, ids) = GrupoCom(n);

        var despesa = grupo.AdicionarDespesa(ids[0], "Jantar", valor, ids);

        Assert.Equal(esperado.Select(e => (decimal)e), despesa.Divisoes.Select(d => d.Valor));
        Assert.Equal(valor, despesa.Divisoes.Sum(d => d.Valor));
        Assert.Equal(ids, despesa.Divisoes.Select(d => d.ParticipanteId));
        Assert.Single(grupo.Despesas);
    }

    [Fact]
    public void Rejeita_pagador_de_fora_do_grupo()
    {
        var (grupo, ids) = GrupoCom(2);
        Assert.Throws<ArgumentException>(() => grupo.AdicionarDespesa(Guid.NewGuid(), "Jantar", 10, ids));
    }

    [Fact]
    public void Rejeita_participante_de_fora_do_grupo()
    {
        var (grupo, ids) = GrupoCom(2);
        Assert.Throws<ArgumentException>(() => grupo.AdicionarDespesa(ids[0], "Jantar", 10, [ids[0], Guid.NewGuid()]));
    }

    [Fact]
    public void Rejeita_divisao_vazia_ou_repetida()
    {
        var (grupo, ids) = GrupoCom(2);
        Assert.Throws<ArgumentException>(() => grupo.AdicionarDespesa(ids[0], "Jantar", 10, []));
        Assert.Throws<ArgumentException>(() => grupo.AdicionarDespesa(ids[0], "Jantar", 10, [ids[0], ids[0]]));
    }

    [Fact]
    public void Rejeita_valor_com_mais_de_duas_casas()
    {
        var (grupo, ids) = GrupoCom(2);
        Assert.Throws<ArgumentException>(() => grupo.AdicionarDespesa(ids[0], "Jantar", 10.005m, ids));
        Assert.Empty(grupo.Despesas);
    }
}
