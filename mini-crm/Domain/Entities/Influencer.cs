namespace mini_crm.Domain.Entities;

public class Influencer : EntidadeComercial
{
    public Guid Id { get; set; }
    public ICollection<InfluencerRedeSocial> RedesSociais { get; set; } = new List<InfluencerRedeSocial>();
    public ICollection<InfluencerContaBancaria> ContasBancarias { get; set; } = new List<InfluencerContaBancaria>();
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}
