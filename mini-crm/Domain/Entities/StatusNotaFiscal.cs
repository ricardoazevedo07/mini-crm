namespace mini_crm.Domain.Entities;

public class StatusNotaFiscal
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ICollection<AgendamentoNotaFiscal> Agendamentos { get; set; } = new List<AgendamentoNotaFiscal>();
}
