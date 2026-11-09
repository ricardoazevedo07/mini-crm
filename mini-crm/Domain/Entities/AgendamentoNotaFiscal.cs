namespace mini_crm.Domain.Entities;

public class AgendamentoNotaFiscal
{
    public long Id { get; set; }
    public long NotaFiscalId { get; set; }
    public DateTime DataPrazoRepasse { get; set; }
    public string DataPrazoRepasseObservacao { get; set; } = string.Empty;
    public DateTime DataPagamento { get; set; }
    public int StatusNotaFiscalId { get; set; }
    public NotaFiscal NotaFiscal { get; set; } = null!;
    public StatusNotaFiscal StatusNotaFiscal { get; set; } = null!;
}
