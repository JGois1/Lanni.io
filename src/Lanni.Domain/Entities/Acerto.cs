namespace Lanni.Domain.Entities;

public class Acerto
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid GrupoId { get; private set; }
    public Guid DeId { get; private set; }
    public Guid ParaId { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime Data { get; private set; } = DateTime.UtcNow;

    public Grupo Grupo { get; private set; } = null!;
    public Participante De { get; private set; } = null!;
    public Participante Para { get; private set; } = null!;

    private Acerto() { }

    internal Acerto(Guid grupoId, Guid deId, Guid paraId, decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do acerto deve ser maior que zero.");
        if (decimal.Round(valor, 2) != valor)
            throw new ArgumentException("O valor do acerto pode ter no máximo 2 casas decimais.");
        if (deId == paraId)
            throw new ArgumentException("Um participante não pode acertar com ele mesmo.");

        GrupoId = grupoId;
        DeId = deId;
        ParaId = paraId;
        Valor = valor;
    }
}
