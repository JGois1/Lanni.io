namespace Lanni.Domain.Entities;

public class DivisaoDespesa
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public Guid DespesaId { get; set; }
    public Guid ParticipanteId { get; set; }
    public decimal Valor { get; set; }

    public Despesa Despesa { get; set; } = null!;
    public Participante Participante { get; set; } = null!;
}