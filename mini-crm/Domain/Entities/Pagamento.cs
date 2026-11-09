namespace mini_crm.Domain.Entities;

public class Pagamento
{
    public long Id { get; set; }
    public long? DespesaId { get; set; }
    public long? ContratoId { get; set; }
    public decimal Valor { get; set; }
    public long EntidadeComercialId { get; set; }
    public DateTime Data { get; set; }
    public long NotaFiscalId { get; set; }
    public Despesa? Despesa { get; set; }
    public Contrato? Contrato { get; set; }
    public EntidadeComercial EntidadeComercial { get; set; } = null!;
    public NotaFiscal NotaFiscal { get; set; } = null!;
}
