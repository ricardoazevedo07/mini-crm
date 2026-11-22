namespace mini_crm.Domain.Entities;

public class Contrato
{
    public long Id { get; set; }
    public long ClienteEntidadeComercialId { get; set; }
    public long AgenciaEntidadeComercialId { get; set; }
    public int AgenteId { get; set; }
    public Divisao Divisao { get; set; }
    public Moeda Moeda { get; set; }
    public long InfluencerEntidadeComercialId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    public AgenciaAgente AgenciaAgente { get; set; } = null!;
    public Influencer Influencer { get; set; } = null!;
    public long? CampanhaId { get; set; }
    public long? InfluencerParticipacaoId { get; set; }
    public StatusContratoCrm StatusCrm { get; set; } = StatusContratoCrm.Rascunho;
    public int Versao { get; private set; } = 1;
    public DateTime? AssinadoEmUtc { get; private set; }
    public string? MotivoCancelamento { get; private set; }
    public string ConteudoContrato { get; set; } = string.Empty;
    public ICollection<VersaoContrato> Versoes { get; set; } = new List<VersaoContrato>();

    public void Assinar(DateTime assinadoEmUtc)
    {
        if (StatusCrm == StatusContratoCrm.Cancelado)
            throw new InvalidOperationException("Contrato cancelado não pode ser assinado.");
        if (StatusCrm == StatusContratoCrm.Assinado)
            throw new InvalidOperationException("Contrato já está assinado.");

        AssinadoEmUtc = assinadoEmUtc.ToUniversalTime();
        StatusCrm = StatusContratoCrm.Assinado;
    }

    public void CriarVersao(DateTime criadaEmUtc)
    {
        if (StatusCrm == StatusContratoCrm.Cancelado)
            throw new InvalidOperationException("Contrato cancelado não pode receber nova versão.");

        Versoes.Add(new VersaoContrato
        {
            Id = Guid.NewGuid(),
            ContratoId = Id,
            Numero = Versao,
            CriadaEmUtc = criadaEmUtc.ToUniversalTime(),
            ConteudoSnapshot = ConteudoContrato
        });

        Versao++;
        StatusCrm = StatusContratoCrm.EmRevisao;
    }

    public void Cancelar(string motivo)
    {
        if (StatusCrm == StatusContratoCrm.Assinado)
            throw new InvalidOperationException("Contrato assinado não pode ser cancelado por este método.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new InvalidOperationException("O motivo do cancelamento é obrigatório.");

        MotivoCancelamento = motivo.Trim();
        StatusCrm = StatusContratoCrm.Cancelado;
    }
    public ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
