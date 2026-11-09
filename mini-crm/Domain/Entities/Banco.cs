namespace mini_crm.Domain.Entities;

public class Banco
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ICollection<ContaBancaria> ContasBancarias { get; set; } = new List<ContaBancaria>();
}
