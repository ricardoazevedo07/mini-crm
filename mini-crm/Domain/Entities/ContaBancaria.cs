namespace mini_crm.Domain.Entities;

public class ContaBancaria
{
    public int Id { get; set; }
    public int BancoId { get; set; }
    public string Agencia { get; set; } = string.Empty;
    public string Conta { get; set; } = string.Empty;
    public Banco Banco { get; set; } = null!;
    public ICollection<InfluencerContaBancaria> Influencers { get; set; } = new List<InfluencerContaBancaria>();
}
