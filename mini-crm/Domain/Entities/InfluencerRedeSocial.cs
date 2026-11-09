namespace mini_crm.Domain.Entities;

public class InfluencerRedeSocial
{
    public long InfluencerEntidadeComercialId { get; set; }
    public int RedeSocialId { get; set; }
    public string Link { get; set; } = string.Empty;
    public Influencer Influencer { get; set; } = null!;
    public RedeSocial RedeSocial { get; set; } = null!;
}
