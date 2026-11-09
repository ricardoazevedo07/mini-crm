namespace mini_crm.Domain.Entities;

public abstract class EntidadeComercial
{
    public long EntidadeComercialId { get; set; }
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
