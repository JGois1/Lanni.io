namespace Lanni.Domain.Entities;

public class Participante
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid GrupoId { get; set; }
    public string Nome { get; set; } = string.Empty;

    public Grupo Grupo { get; set; } = null!;
}