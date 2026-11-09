namespace mini_crm.Domain.Entities;

public class Despesa
{
    public long Id { get; set; }
    public long ContratoId { get; set; }
    public decimal Valor { get; set; }
    public string Observacao { get; set; } = string.Empty;
    public Contrato Contrato { get; set; } = null!;
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
}
