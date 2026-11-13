using Lanni.Domain.Entities;

namespace Lanni.Domain.Tests;

public class GrupoAcertoTests
{
    private readonly Grupo _grupo = new("Casa");
    private readonly Guid _ana;
    private readonly Guid _bia;

    public GrupoAcertoTests()
    {
        _ana = _grupo.AdicionarParticipante("Ana").Id;
        _bia = _grupo.AdicionarParticipante("Bia").Id;
    }

    [Fact]
    public void Registra_acerto_entre_participantes_do_grupo()
    {
        var acerto = _grupo.RegistrarAcerto(_ana, _bia, 30m);

        Assert.Equal((_ana, _bia, 30m), (acerto.DeId, acerto.ParaId, acerto.Valor));
        Assert.Single(_grupo.Acertos);
    }

    [Fact]
    public void Rejeita_acerto_invalido()
    {
        Assert.Throws<ArgumentException>(() => _grupo.RegistrarAcerto(_ana, Guid.NewGuid(), 30m));
        Assert.Throws<ArgumentException>(() => _grupo.RegistrarAcerto(_ana, _ana, 30m));
        Assert.Throws<ArgumentException>(() => _grupo.RegistrarAcerto(_ana, _bia, 0m));
        Assert.Throws<ArgumentException>(() => _grupo.RegistrarAcerto(_ana, _bia, 1.005m));
        Assert.Empty(_grupo.Acertos);
    }
}
