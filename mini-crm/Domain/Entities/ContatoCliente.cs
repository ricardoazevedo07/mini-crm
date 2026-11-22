namespace mini_crm.Domain.Entities;

public class ContatoCliente
{
    public Guid Id { get; set; }
    public long ClienteEntidadeComercialId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Cargo { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public bool Principal { get; set; }
    public Cliente? Cliente { get; set; }

    public void Atualizar(string nome, string? cargo, string email, string? telefone, bool principal)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new InvalidOperationException("O nome do contato é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new InvalidOperationException("O email do contato é obrigatório.");

        Nome = nome.Trim();
        Cargo = string.IsNullOrWhiteSpace(cargo) ? null : cargo.Trim();
        Email = email.Trim();
        Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone.Trim();
        Principal = principal;
    }
}
