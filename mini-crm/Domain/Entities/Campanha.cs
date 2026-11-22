namespace mini_crm.Domain.Entities;

public enum TipoTaxa
{
    Fixa = 1,
    Percentual = 2
}

public class Campanha
{
    public long Id { get; set; }
    public long OportunidadeId { get; set; }
    public long ClienteEntidadeComercialId { get; set; }
    public long ResponsavelId { get; set; }
    public string Frente { get; private set; } = string.Empty;
    public DateTime DataInicio { get; private set; }
    public DateTime? DataFim { get; private set; }
    public Dinheiro TotalAprovado { get; private set; }
    public TipoTaxa TipoTaxa { get; private set; }
    public decimal Taxa { get; private set; }
    public StatusCampanha Status { get; private set; } = StatusCampanha.Planejada;

    public void Configurar(string frente, DateTime dataInicio, DateTime? dataFim, Dinheiro totalAprovado, TipoTaxa tipoTaxa, decimal taxa)
    {
        if (string.IsNullOrWhiteSpace(frente))
            throw new InvalidOperationException("A frente da campanha é obrigatória.");
        if (dataFim.HasValue && dataFim.Value < dataInicio)
            throw new InvalidOperationException("A data fim não pode ser anterior à data início.");
        if (tipoTaxa == TipoTaxa.Percentual && (taxa < 0 || taxa > 100))
            throw new InvalidOperationException("Taxa percentual deve estar entre 0 e 100.");
        if (tipoTaxa == TipoTaxa.Fixa && taxa < 0)
            throw new InvalidOperationException("Taxa fixa não pode ser negativa.");

        Frente = frente.Trim();
        DataInicio = dataInicio;
        DataFim = dataFim;
        TotalAprovado = totalAprovado;
        TipoTaxa = tipoTaxa;
        Taxa = tipoTaxa == TipoTaxa.Percentual ? decimal.Round(taxa, 4) : decimal.Round(taxa, 2);
    }

    public void Iniciar()
    {
        if (Status != StatusCampanha.Planejada)
            throw new InvalidOperationException("Apenas campanha planejada pode ser iniciada.");
        Status = StatusCampanha.EmExecucao;
    }

    public void EnviarParaAprovacao()
    {
        if (Status != StatusCampanha.EmExecucao)
            throw new InvalidOperationException("A campanha precisa estar em execução.");
        Status = StatusCampanha.EmAprovacao;
    }

    public void Concluir(bool checklistOperacionalCompleto, Dinheiro saldoPendente)
    {
        if (Status != StatusCampanha.EmAprovacao)
            throw new InvalidOperationException("A campanha precisa estar em aprovação.");
        if (!checklistOperacionalCompleto)
            throw new InvalidOperationException("O checklist operacional precisa estar completo.");
        if (saldoPendente.Valor != 0)
            throw new InvalidOperationException("A campanha possui saldo financeiro pendente.");

        Status = StatusCampanha.Concluida;
        DataFim ??= DateTime.UtcNow;
    }

    public void Cancelar(string motivo)
    {
        if (Status == StatusCampanha.Concluida)
            throw new InvalidOperationException("Campanha concluída não pode ser cancelada.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new InvalidOperationException("O motivo do cancelamento é obrigatório.");
        Status = StatusCampanha.Cancelada;
    }
}
