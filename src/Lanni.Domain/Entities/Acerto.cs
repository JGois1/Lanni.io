namespace Lanni.Domain.Entities;

public class Acerto
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid GrupoId { get; set; }
    public Guid DeId { get; set; }
    public Guid ParaId { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;

    public Grupo Grupo { get; set; } = null!;
    public Participante De { get; set; } = null!;
    public Participante Para { get; set; } = null!;
}