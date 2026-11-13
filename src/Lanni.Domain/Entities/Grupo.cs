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

    public Despesa AdicionarDespesa(Guid pagadorId, string descricao, decimal valor, IReadOnlyList<Guid> participanteIds)
    {
        if (_participantes.All(p => p.Id != pagadorId))
            throw new ArgumentException("O pagador não pertence a este grupo.");
        if (participanteIds.Count == 0)
            throw new ArgumentException("Informe ao menos um participante para dividir a despesa.");
        if (participanteIds.Distinct().Count() != participanteIds.Count)
            throw new ArgumentException("Há participantes repetidos na divisão.");
        if (participanteIds.Any(id => _participantes.All(p => p.Id != id)))
            throw new ArgumentException("Há participantes na divisão que não pertencem a este grupo.");

        var despesa = new Despesa(Id, pagadorId, descricao, valor);

        // Divide em centavos para baixo; o centavo que sobra fica com o último (100 / 3 = 33,33 + 33,33 + 33,34).
        var parte = Math.Floor(valor / participanteIds.Count * 100) / 100;
        var resto = valor - parte * participanteIds.Count;

        for (var i = 0; i < participanteIds.Count; i++)
        {
            despesa.Divisoes.Add(new DivisaoDespesa
            {
                DespesaId = despesa.Id,
                ParticipanteId = participanteIds[i],
                Valor = i == participanteIds.Count - 1 ? parte + resto : parte
            });
        }

        _despesas.Add(despesa);
        return despesa;
    }

    public Acerto RegistrarAcerto(Guid deId, Guid paraId, decimal valor)
    {
        if (_participantes.All(p => p.Id != deId) || _participantes.All(p => p.Id != paraId))
            throw new ArgumentException("Os participantes do acerto precisam pertencer a este grupo.");

        var acerto = new Acerto(Id, deId, paraId, valor);
        _acertos.Add(acerto);
        return acerto;
    }
}