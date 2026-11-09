using Microsoft.EntityFrameworkCore;
using mini_crm.Application.Common;
using mini_crm.Domain.IRepositories;
using mini_crm.Infrastructure.DbContext;

namespace mini_crm.Infrastructure.Repositories;

public class EfRepository<TEntity>(AppDbContext context) : IRepository<TEntity>, IPagedRepository<TEntity>
    where TEntity : class
{
    protected AppDbContext Context { get; } = context;
    protected DbSet<TEntity> Set => Context.Set<TEntity>();

    public virtual async Task<TEntity?> FindAsync(object[] keyValues, CancellationToken cancellationToken = default) =>
        await Set.FindAsync(keyValues, cancellationToken);

    public virtual async Task<IReadOnlyList<TEntity>> ListAsync(CancellationToken cancellationToken = default) =>
        await Set.AsNoTracking().ToListAsync(cancellationToken);

    public virtual async Task<PagedResult<TEntity>> PageAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var page = request.NormalizedPage;
        var pageSize = request.NormalizedPageSize;
        var query = Set.AsNoTracking();
        var keyProperty = Context.Model.FindEntityType(typeof(TEntity))?.FindPrimaryKey()?.Properties.FirstOrDefault()?.Name;
        if (keyProperty is not null)
        {
            query = query.OrderBy(entity => EF.Property<object>(entity, keyProperty));
        }
        var total = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PagedResult<TEntity>(items, page, pageSize, total);
    }

    public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        await Set.AddAsync(entity, cancellationToken);

    public virtual void Update(TEntity entity) => Set.Update(entity);

    public virtual void Remove(TEntity entity) => Set.Remove(entity);
}
