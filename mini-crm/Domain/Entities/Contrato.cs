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
    public ICollection<Despesa> Despesas { get; set; } = new List<Despesa>();
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
