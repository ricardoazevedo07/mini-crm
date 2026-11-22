namespace mini_crm.Domain.Entities;

public class ContaReceber
{
    public long Id { get; set; }
    public long CampanhaId { get; set; }
    public Dinheiro Valor { get; private set; }
    public Dinheiro ValorAprovado { get; private set; }
    public Dinheiro Saldo => new(ValorAprovado.Valor - ValorRecebidoAcumulado, ValorAprovado.CodigoMoeda);
    public decimal ValorRecebidoAcumulado { get; private set; }
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

    public void RegistrarRecebimento(Dinheiro valor)
    {
        if (valor.CodigoMoeda != ValorAprovado.CodigoMoeda) throw new InvalidOperationException("Moeda incompatível.");
        if (valor.Valor <= 0) throw new InvalidOperationException("O recebimento deve ser positivo.");
        if (ValorRecebidoAcumulado + valor.Valor > ValorAprovado.Valor) throw new InvalidOperationException("Recebimento excede o saldo.");

        ValorRecebidoAcumulado += valor.Valor;
        Status = ValorRecebidoAcumulado == ValorAprovado.Valor ? StatusObrigacao.Liquidada : StatusObrigacao.Parcial;
    }
}
