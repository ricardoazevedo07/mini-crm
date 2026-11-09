using mini_crm.Application.Common;

namespace mini_crm.Domain.IRepositories;

public interface IPagedRepository<TEntity> where TEntity : class
{
    Task<PagedResult<TEntity>> PageAsync(PageRequest request, CancellationToken cancellationToken = default);
}
