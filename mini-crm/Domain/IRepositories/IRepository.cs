namespace mini_crm.Domain.IRepositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}
