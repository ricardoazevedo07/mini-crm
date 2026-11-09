namespace mini_crm.Domain.Entities;

public class Cliente : EntidadeComercial
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}
