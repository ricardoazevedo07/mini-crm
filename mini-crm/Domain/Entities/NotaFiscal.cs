namespace mini_crm.Domain.Entities;

public class NotaFiscal
{
    public long Id { get; set; }
    public int FormatoNotaFiscalId { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public bool Enviada { get; set; }
    public FormatoNotaFiscal FormatoNotaFiscal { get; set; } = null!;
    public ICollection<Pagamento> Pagamentos { get; set; } = new List<Pagamento>();
    public ICollection<AgendamentoNotaFiscal> Agendamentos { get; set; } = new List<AgendamentoNotaFiscal>();
}
