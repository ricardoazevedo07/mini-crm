namespace mini_crm.Domain.Entities;

public class FormatoNotaFiscal
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ICollection<NotaFiscal> NotasFiscais { get; set; } = new List<NotaFiscal>();
}
