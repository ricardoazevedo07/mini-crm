namespace mini_crm.Domain.Entities;

public class Cliente : EntidadeComercial
{
    public Guid Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public string Segmento { get; set; } = string.Empty;
    public string PreferenciaIdioma { get; set; } = string.Empty;
    public bool Ativo { get; private set; } = true;
    public ICollection<ContatoCliente> Contatos { get; set; } = new List<ContatoCliente>();
    public ICollection<Contrato> Contratos { get; set; } = new List<Contrato>();

    public void AtualizarDadosComerciais(string nome, string cnpj, string segmento, string preferenciaIdioma)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new InvalidOperationException("O nome do cliente é obrigatório.");

        Nome = nome.Trim();
        Cnpj = NormalizarCnpj(cnpj);
        Segmento = segmento?.Trim() ?? string.Empty;
        PreferenciaIdioma = preferenciaIdioma?.Trim() ?? string.Empty;
    }

    public void Arquivar()
    {
        Ativo = false;
    }

    public void Reativar()
    {
        Ativo = true;
    }

    public static string NormalizarCnpj(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
            throw new InvalidOperationException("O CNPJ é obrigatório.");

        var normalized = new string(cnpj.Where(char.IsDigit).ToArray());
        if (normalized.Length != 14)
            throw new InvalidOperationException("O CNPJ deve conter 14 dígitos.");

        return normalized;
    }
}
