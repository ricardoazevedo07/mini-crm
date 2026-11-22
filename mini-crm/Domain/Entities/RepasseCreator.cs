namespace mini_crm.Domain.Entities;

public class RepasseCreator
{
    public Guid Id { get; set; }
    public Guid ParticipacaoCampanhaId { get; set; }
    public long ContaPagarId { get; set; }
    public Dinheiro Valor { get; private set; }
    public StatusObrigacao Status { get; private set; } = StatusObrigacao.Pendente;

    public void Configurar(Dinheiro valor)
    {
        Valor = valor;
    }
}

public class LancamentoLiquidacao
{
    public Guid Id { get; set; }
    public long ContaReceberId { get; set; }
    public long? ContaPagarId { get; set; }
    public Guid? RepasseCreatorId { get; set; }
    public Dinheiro Valor { get; private set; }
    public DateTime LiquidadoEmUtc { get; private set; }
    public string Referencia { get; private set; } = string.Empty;

    public void Configurar(Dinheiro valor, DateTime liquidadoEmUtc, string referencia)
    {
        if (valor.Valor <= 0) throw new InvalidOperationException("O lançamento deve ser positivo.");
        if (string.IsNullOrWhiteSpace(referencia)) throw new InvalidOperationException("A referência do lançamento é obrigatória.");

        Valor = valor;
        LiquidadoEmUtc = liquidadoEmUtc.ToUniversalTime();
        Referencia = referencia.Trim();
    }
}
