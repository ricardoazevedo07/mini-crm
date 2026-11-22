using mini_crm.Application.Common;
using mini_crm.Application.Contracts;
using mini_crm.Domain.Entities;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Application.UseCases.Customers;

public sealed class CustomerUseCases(IClienteRepository clientes, IUnitOfWork unitOfWork)
{
    public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        ValidateCustomer(request.Name, request.Cnpj);
        var normalizedCnpj = Cliente.NormalizarCnpj(request.Cnpj);
        var existing = await clientes.ListAsync(cancellationToken);
        if (existing.Any(x => x.Cnpj == normalizedCnpj && x.Ativo))
            throw new InvalidOperationException("Já existe um cliente ativo com este CNPJ.");

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Codigo = request.Codigo?.Trim() ?? string.Empty
        };
        cliente.AtualizarDadosComerciais(request.Name, request.Cnpj, request.Segmento ?? string.Empty, request.PreferenciaIdioma ?? string.Empty);

        await clientes.AddAsync(cliente, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(cliente);
    }

    public async Task<CustomerResponse?> GetAsync(long id, CancellationToken cancellationToken = default)
    {
        var cliente = await clientes.FindAsync([id], cancellationToken);
        return cliente is null ? null : Map(cliente);
    }

    public async Task<PagedResult<CustomerResponse>> SearchAsync(string? query, bool? active, PageRequest pageRequest, CancellationToken cancellationToken = default)
    {
        var clientesAtivos = await clientes.ListAsync(cancellationToken);
        IEnumerable<Cliente> result = clientesAtivos;
        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();
            result = result.Where(x => x.Nome.Contains(term, StringComparison.OrdinalIgnoreCase) || x.Cnpj.Contains(term));
        }
        if (active.HasValue)
            result = result.Where(x => x.Ativo == active.Value);

        pageRequest.Validate();
        var ordered = result.OrderBy(x => x.EntidadeComercialId).ToList();
        var total = ordered.Count;
        var items = ordered.Skip((pageRequest.Page - 1) * pageRequest.PageSize)
            .Take(pageRequest.PageSize).Select(Map).ToList();
        return new PagedResult<CustomerResponse>(items, pageRequest.Page, pageRequest.PageSize, total);
    }

    public async Task<CustomerResponse> UpdateAsync(long id, UpdateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        ValidateCustomer(request.Name, request.Cnpj);
        var cliente = await clientes.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException();
        var normalizedCnpj = Cliente.NormalizarCnpj(request.Cnpj);
        var existing = await clientes.ListAsync(cancellationToken);
        if (existing.Any(x => x.EntidadeComercialId != id && x.Cnpj == normalizedCnpj && x.Ativo))
            throw new InvalidOperationException("Já existe um cliente ativo com este CNPJ.");

        cliente.AtualizarDadosComerciais(request.Name, request.Cnpj, request.Segmento ?? string.Empty, request.PreferenciaIdioma ?? string.Empty);
        clientes.Update(cliente);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Map(cliente);
    }

    public async Task ArchiveAsync(long id, CancellationToken cancellationToken = default)
    {
        var cliente = await clientes.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException();
        if (cliente.Contratos.Any())
            throw new InvalidOperationException("Cliente com contratos não pode ser arquivado por este endpoint.");
        cliente.Arquivar();
        clientes.Update(cliente);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<CustomerContactResponse> AddContactAsync(long customerId, CreateCustomerContactRequest request, CancellationToken cancellationToken = default)
    {
        var cliente = await clientes.FindAsync([customerId], cancellationToken) ?? throw new KeyNotFoundException();
        var contato = new ContatoCliente { Id = Guid.NewGuid(), ClienteEntidadeComercialId = customerId };
        contato.Atualizar(request.Name, request.Cargo, request.Email, request.Telefone, request.Principal);
        cliente.Contatos.Add(contato);
        clientes.Update(cliente);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapContact(contato);
    }

    public async Task<IReadOnlyList<CustomerContactResponse>> ListContactsAsync(long customerId, CancellationToken cancellationToken = default)
    {
        var cliente = await clientes.FindAsync([customerId], cancellationToken) ?? throw new KeyNotFoundException();
        return cliente.Contatos.Select(MapContact).ToList();
    }

    public async Task<CustomerContactResponse?> GetContactAsync(long customerId, Guid contactId, CancellationToken cancellationToken = default)
    {
        var cliente = await clientes.FindAsync([customerId], cancellationToken) ?? throw new KeyNotFoundException();
        var contato = cliente.Contatos.FirstOrDefault(x => x.Id == contactId);
        return contato is null ? null : MapContact(contato);
    }

    public async Task<CustomerContactResponse> UpdateContactAsync(long customerId, Guid contactId, UpdateCustomerContactRequest request, CancellationToken cancellationToken = default)
    {
        var cliente = await clientes.FindAsync([customerId], cancellationToken) ?? throw new KeyNotFoundException();
        var contato = cliente.Contatos.FirstOrDefault(x => x.Id == contactId) ?? throw new KeyNotFoundException();
        contato.Atualizar(request.Name, request.Cargo, request.Email, request.Telefone, request.Principal);
        clientes.Update(cliente);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return MapContact(contato);
    }

    private static void ValidateCustomer(string name, string cnpj)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        _ = Cliente.NormalizarCnpj(cnpj);
    }

    private static CustomerResponse Map(Cliente cliente) => new(cliente.EntidadeComercialId, cliente.Id, cliente.Codigo, cliente.Nome, cliente.Cnpj,
        cliente.Segmento, cliente.PreferenciaIdioma, cliente.Ativo);

    private static CustomerContactResponse MapContact(ContatoCliente contato) => new(contato.Id, contato.Nome, contato.Cargo, contato.Email, contato.Telefone, contato.Principal);
}
