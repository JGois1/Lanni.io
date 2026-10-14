namespace Lanni.Domain.Entities;

public class Participante
{
    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public Guid GrupoId { get; private set; }
    public string Nome { get; private set; } = string.Empty;

    public Grupo Grupo { get; private set; } = null!;

    private Participante() { }

    internal Participante(Guid grupoId, string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do participante é obrigatório.");

        nome = nome.Trim();
        if (nome.Length > 100)
            throw new ArgumentException("O nome do participante pode ter no máximo 100 caracteres.");

        GrupoId = grupoId;
        Nome = nome;
    }
}