namespace mini_crm.Domain.Entities;

public class AgenciaAgente
{
    public long AgenciaEntidadeComercialId { get; set; }
    public int AgenteId { get; set; }
    public Agencia Agencia { get; set; } = null!;
    public Agente Agente { get; set; } = null!;
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();
}
