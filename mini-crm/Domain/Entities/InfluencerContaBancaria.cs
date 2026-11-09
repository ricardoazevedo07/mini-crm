namespace mini_crm.Domain.Entities;

public class InfluencerContaBancaria
{
    public long InfluencerEntidadeComercialId { get; set; }
    public int ContaBancariaId { get; set; }
    public Influencer Influencer { get; set; } = null!;
    public ContaBancaria ContaBancaria { get; set; } = null!;
}
