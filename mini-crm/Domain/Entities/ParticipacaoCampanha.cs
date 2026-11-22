namespace mini_crm.Domain.Entities;

public class ParticipacaoCampanha
{
    public Guid Id { get; set; }
    public long CampanhaId { get; set; }
    public long InfluencerEntidadeComercialId { get; set; }
    public Dinheiro CompensacaoNegociada { get; private set; }
    public Dinheiro CompensacaoAprovada { get; private set; }
    public string Termos { get; private set; } = string.Empty;
    public StatusParticipacao Status { get; private set; } = StatusParticipacao.Convidado;
    public long? RepasseId { get; private set; }

    public void Negociar(Dinheiro compensacao, string termos)
    {
        if (Status != StatusParticipacao.Convidado)
            throw new InvalidOperationException("Participação precisa estar convidada para negociação.");
        if (string.IsNullOrWhiteSpace(termos))
            throw new InvalidOperationException("Os termos da participação são obrigatórios.");

        CompensacaoNegociada = compensacao;
        Termos = termos.Trim();
        Status = StatusParticipacao.Negotiado;
    }

    public void Aprovar(Dinheiro compensacaoAprovada)
    {
        if (Status != StatusParticipacao.Negotiado)
            throw new InvalidOperationException("Participação precisa estar negociada.");

        CompensacaoAprovada = compensacaoAprovada;
        Status = StatusParticipacao.Aprovado;
    }

    public void Ativar()
    {
        if (Status != StatusParticipacao.Aprovado)
            throw new InvalidOperationException("Participação precisa estar aprovada.");
        Status = StatusParticipacao.Ativo;
    }

    public void Cancelar(string motivo, long? repasseId = null)
    {
        if (Status == StatusParticipacao.Concluido)
            throw new InvalidOperationException("Participação concluída não pode ser cancelada.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new InvalidOperationException("O motivo do cancelamento é obrigatório.");

        RepasseId = repasseId;
        Status = StatusParticipacao.Cancelado;
    }
}
