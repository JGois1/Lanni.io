namespace Lanni.Domain.Entities;

public class Grupo
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Nome { get; set; } = string.Empty;
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public List<Participante> Participantes { get; set; } = new();
    public List<Despesa> Despesas { get; set; } = new();
    public List<Acerto> Acertos { get; set; } = new();
}