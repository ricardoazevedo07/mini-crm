using Microsoft.EntityFrameworkCore;
using mini_crm.Domain.IRepositories;
using mini_crm.Infrastructure.DbContext;

namespace mini_crm.Infrastructure.Repositories;

public sealed class EntityReferenceRepository(AppDbContext context) : IEntityReferenceRepository
{
    public async Task<bool> ExistsAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken = default)
        where TEntity : class => await context.Set<TEntity>().FindAsync(keyValues, cancellationToken) is not null;

    public Task<bool> AssociationExistsAsync<TAssociation>(object[] keyValues, CancellationToken cancellationToken = default)
        where TAssociation : class => ExistsAsync<TAssociation>(keyValues, cancellationToken);
}
