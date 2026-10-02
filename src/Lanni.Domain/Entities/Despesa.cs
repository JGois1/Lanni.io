namespace Lanni.Domain.Entities;

public class Despesa
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid GrupoId { get; set; }
    public Guid PagadorId { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime Data { get; set; } = DateTime.UtcNow;

    public Grupo Grupo { get; set; } = null!;
    public Participante Pagador { get; set; } = null!;
    public List<DivisaoDespesa> Divisoes { get; set; } = new();
}