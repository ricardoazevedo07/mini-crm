namespace mini_crm.Domain.IRepositories;

public interface IEntityReferenceRepository
{
    Task<bool> ExistsAsync<TEntity>(object[] keyValues, CancellationToken cancellationToken = default) where TEntity : class;
    Task<bool> AssociationExistsAsync<TAssociation>(object[] keyValues, CancellationToken cancellationToken = default) where TAssociation : class;
}
