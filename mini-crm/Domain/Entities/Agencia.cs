namespace mini_crm.Domain.Entities;

public class Agencia : EntidadeComercial
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public ICollection<AgenciaAgente> Agentes { get; set; } = new List<AgenciaAgente>();
}
