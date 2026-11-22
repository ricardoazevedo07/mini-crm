namespace mini_crm.Application.Contracts;

public sealed record CreateCustomerRequest(string Name, string Cnpj, string? Segmento, string? PreferenciaIdioma, string? Codigo);
public sealed record UpdateCustomerRequest(string Name, string Cnpj, string? Segmento, string? PreferenciaIdioma);
public sealed record CustomerResponse(long Id, Guid BusinessId, string Code, string Name, string Cnpj, string? Segmento, string? PreferenciaIdioma, bool Active);
public sealed record CreateCustomerContactRequest(string Name, string? Cargo, string Email, string? Telefone, bool Principal);
public sealed record UpdateCustomerContactRequest(string Name, string? Cargo, string Email, string? Telefone, bool Principal);
public sealed record CustomerContactResponse(Guid Id, string Name, string? Cargo, string Email, string? Telefone, bool Principal);
