namespace Lanni.Domain.Entities;

public class Grupo
{
    private readonly List<Participante> _participantes = new();
    private readonly List<Despesa> _despesas = new();
    private readonly List<Acerto> _acertos = new();

    public Guid Id { get; private set; } = Guid.CreateVersion7();
    public string Nome { get; private set; } = string.Empty;
    public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

    public IReadOnlyCollection<Participante> Participantes => _participantes;
    public IReadOnlyCollection<Despesa> Despesas => _despesas;
    public IReadOnlyCollection<Acerto> Acertos => _acertos;

    private Grupo() { }

    public Grupo(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do grupo é obrigatório.");

        nome = nome.Trim();
        if (nome.Length > 100)
            throw new ArgumentException("O nome do grupo pode ter no máximo 100 caracteres.");

        Nome = nome;
    }

    public Participante AdicionarParticipante(string nome)
    {
        var participante = new Participante(Id, nome);

        if (_participantes.Any(p => p.Nome.Equals(participante.Nome, StringComparison.OrdinalIgnoreCase)))
            throw new ArgumentException($"Já existe um participante chamado '{participante.Nome}' neste grupo.");

        _participantes.Add(participante);
        return participante;
    }
}