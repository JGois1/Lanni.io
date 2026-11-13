namespace Lanni.Domain.Entities;

public class Despesa
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid GrupoId { get; private set; }
    public Guid PagadorId { get; private set; }
    public string Descricao { get; private set; } = string.Empty;
    public decimal Valor { get; private set; }
    public DateTime Data { get; private set; } = DateTime.UtcNow;

    public Grupo Grupo { get; set; } = null!;
    public Participante Pagador { get; set; } = null!;
    public List<DivisaoDespesa> Divisoes { get; set; } = new();

    private Despesa() { }

    public Despesa(Guid grupoId, Guid pagadorId, string descricao, decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor da despesa deve ser maior que zero.");
        if (decimal.Round(valor, 2) != valor)
            throw new ArgumentException("O valor da despesa pode ter no máximo 2 casas decimais.");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição é obrigatória.");

        descricao = descricao.Trim();
        if (descricao.Length > 200)
            throw new ArgumentException("A descrição pode ter no máximo 200 caracteres.");

        GrupoId = grupoId;
        PagadorId = pagadorId;
        Descricao = descricao;
        Valor = valor;
    }
}