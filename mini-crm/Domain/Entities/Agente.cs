namespace mini_crm.Domain.Entities;

public class Agente
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Contato { get; set; } = string.Empty;
    public ICollection<AgenciaAgente> Agencias { get; set; } = new List<AgenciaAgente>();
}
