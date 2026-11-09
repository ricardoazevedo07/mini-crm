namespace mini_crm.Domain.Entities;

public class RedeSocial
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ICollection<InfluencerRedeSocial> Influencers { get; set; } = new List<InfluencerRedeSocial>();
}
