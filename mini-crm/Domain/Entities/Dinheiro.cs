namespace mini_crm.Domain.Entities;

public readonly record struct Dinheiro
{
    public decimal Valor { get; }
    public string CodigoMoeda { get; }

    public Dinheiro(decimal valor, string codigoMoeda)
    {
        if (valor < 0)
            throw new InvalidOperationException("O valor monetário não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(codigoMoeda))
            throw new InvalidOperationException("O código da moeda é obrigatório.");

        var normalized = codigoMoeda.Trim().ToUpperInvariant();
        if (normalized.Length != 3 || !normalized.All(char.IsAsciiLetter))
            throw new InvalidOperationException("O código da moeda deve conter três letras ISO 4217.");

        Valor = decimal.Round(valor, 2);
        CodigoMoeda = normalized;
    }

    public Dinheiro Somar(Dinheiro outro)
    {
        GarantirMesmaMoeda(outro);
        return new Dinheiro(Valor + outro.Valor, CodigoMoeda);
    }

    public Dinheiro Subtrair(Dinheiro outro)
    {
        GarantirMesmaMoeda(outro);
        return new Dinheiro(Valor - outro.Valor, CodigoMoeda);
    }

    private void GarantirMesmaMoeda(Dinheiro outro)
    {
        if (!CodigoMoeda.Equals(outro.CodigoMoeda, StringComparison.Ordinal))
            throw new InvalidOperationException("Operações monetárias exigem moedas iguais.");
    }
}
