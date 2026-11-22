namespace mini_crm.Domain.Entities;

public class ContaPagar
{
    public long Id { get; set; }
    public long CampanhaId { get; set; }
    public Guid? ParticipacaoCampanhaId { get; set; }
    public Dinheiro Valor { get; private set; }
    public Dinheiro ValorAprovado { get; private set; }
    public decimal ValorPagoAcumulado { get; private set; }
    public DateTime DataVencimentoUtc { get; private set; }
    public StatusObrigacao Status { get; private set; } = StatusObrigacao.Pendente;

    public void Configurar(Dinheiro valor, Dinheiro valorAprovado, DateTime vencimentoUtc)
    {
        if (valor.CodigoMoeda != valorAprovado.CodigoMoeda)
            throw new InvalidOperationException("Os valores da conta devem usar a mesma moeda.");
        if (valorAprovado.Valor > valor.Valor)
            throw new InvalidOperationException("O valor aprovado não pode exceder o valor original.");

        Valor = valor;
        ValorAprovado = valorAprovado;
        DataVencimentoUtc = vencimentoUtc.ToUniversalTime();
    }

    public Dinheiro Saldo()
    {
        return new Dinheiro(ValorAprovado.Valor - ValorPagoAcumulado, ValorAprovado.CodigoMoeda);
    }

    public void RegistrarPagamento(Dinheiro valor)
    {
        if (valor.CodigoMoeda != ValorAprovado.CodigoMoeda) throw new InvalidOperationException("Moeda incompatível.");
        if (valor.Valor <= 0) throw new InvalidOperationException("O pagamento deve ser positivo.");
        if (ValorPagoAcumulado + valor.Valor > ValorAprovado.Valor) throw new InvalidOperationException("Pagamento excede o saldo.");

        ValorPagoAcumulado += valor.Valor;
        Status = ValorPagoAcumulado == ValorAprovado.Valor ? StatusObrigacao.Liquidada : StatusObrigacao.Parcial;
    }
}
