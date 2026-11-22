namespace mini_crm.Domain.Entities;

public class Oportunidade
{
    public long Id { get; set; }
    public long ClienteEntidadeComercialId { get; set; }
    public long ResponsavelId { get; set; }
    public string Briefing { get; private set; } = string.Empty;
    public Dinheiro OrcamentoEstimado { get; private set; }
    public StatusOportunidade Status { get; private set; } = StatusOportunidade.Nova;
    public string? MotivoPerda { get; private set; }
    public long? CampanhaId { get; private set; }

    public void Qualificar(string briefing, Dinheiro orcamentoEstimado)
    {
        if (Status != StatusOportunidade.Nova)
            throw new InvalidOperationException("Apenas oportunidades novas podem ser qualificadas.");
        if (string.IsNullOrWhiteSpace(briefing))
            throw new InvalidOperationException("O briefing é obrigatório.");

        Briefing = briefing.Trim();
        OrcamentoEstimado = orcamentoEstimado;
        Status = StatusOportunidade.Qualificada;
    }

    public void RegistrarProposta(Dinheiro orcamentoEstimado)
    {
        if (Status != StatusOportunidade.Qualificada)
            throw new InvalidOperationException("A oportunidade precisa estar qualificada.");

        OrcamentoEstimado = orcamentoEstimado;
        Status = StatusOportunidade.Proposta;
    }

    public void MarcarGanha()
    {
        if (Status != StatusOportunidade.Proposta)
            throw new InvalidOperationException("Apenas proposta pode ser marcada como ganha.");

        Status = StatusOportunidade.Ganha;
    }

    public void Perder(string motivo)
    {
        if (Status == StatusOportunidade.Ganha || Status == StatusOportunidade.Fechada)
            throw new InvalidOperationException("Oportunidade ganha ou fechada não pode ser perdida.");
        if (string.IsNullOrWhiteSpace(motivo))
            throw new InvalidOperationException("O motivo da perda é obrigatório.");

        MotivoPerda = motivo.Trim();
        Status = StatusOportunidade.Perdida;
    }

    public void VincularCampanha(long campanhaId)
    {
        if (Status != StatusOportunidade.Ganha)
            throw new InvalidOperationException("A campanha só pode ser criada a partir de oportunidade ganha.");
        if (CampanhaId.HasValue)
            throw new InvalidOperationException("Esta oportunidade já foi convertida em campanha.");

        CampanhaId = campanhaId;
        Status = StatusOportunidade.Fechada;
    }
}
