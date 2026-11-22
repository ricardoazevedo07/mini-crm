namespace mini_crm.Domain.Entities;

public class VersaoContrato
{
    public Guid Id { get; set; }
    public long ContratoId { get; set; }
    public int Numero { get; set; }
    public DateTime CriadaEmUtc { get; set; }
    public string ConteudoSnapshot { get; set; } = string.Empty;
}
