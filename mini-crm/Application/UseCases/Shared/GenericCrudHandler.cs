using System.Reflection;
using System.Text.Json;
using mini_crm.Application.Abstractions;
using mini_crm.Application.Common;
using mini_crm.Domain.IRepositories;

namespace mini_crm.Application.UseCases.Shared;

public sealed class GenericCrudHandler<TEntity>(IRepository<TEntity> repository, IPagedRepository<TEntity> pagedRepository, IUnitOfWork unitOfWork) :
    IRequestHandler<CrudRequest, IReadOnlyDictionary<string, object?>>
    where TEntity : class, new()
{
    public async Task<OperationResult<IReadOnlyDictionary<string, object?>>> HandleAsync(CrudRequest request, CancellationToken cancellationToken = default)
    {
        var entity = new TEntity();
        ApplyPayload(entity, request.Payload, isUpdate: false);
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return OperationResult<IReadOnlyDictionary<string, object?>>.Success(EntityResponseMapper.Map(entity));
    }

    public async Task<OperationResult<IReadOnlyDictionary<string, object?>>> UpdateAsync(object[] keys, CrudRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await repository.FindAsync(keys, cancellationToken);
        if (entity is null)
        {
            return OperationResult<IReadOnlyDictionary<string, object?>>.Failure(ApplicationErrors.NotFound($"{typeof(TEntity).Name} not found."));
        }

        ApplyPayload(entity, request.Payload, isUpdate: true);
        repository.Update(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return OperationResult<IReadOnlyDictionary<string, object?>>.Success(EntityResponseMapper.Map(entity));
    }

    public async Task<OperationResult<IReadOnlyDictionary<string, object?>>> GetAsync(object[] keys, CancellationToken cancellationToken = default)
    {
        var entity = await repository.FindAsync(keys, cancellationToken);
        return entity is null
            ? OperationResult<IReadOnlyDictionary<string, object?>>.Failure(ApplicationErrors.NotFound($"{typeof(TEntity).Name} not found."))
            : OperationResult<IReadOnlyDictionary<string, object?>>.Success(EntityResponseMapper.Map(entity));
    }

    public async Task<OperationResult<PagedResult<IReadOnlyDictionary<string, object?>>>> ListAsync(PageRequest request, CancellationToken cancellationToken = default)
    {
        var page = await pagedRepository.PageAsync(request, cancellationToken);
        var mapped = page.Items.Select(EntityResponseMapper.Map).ToArray();
        return OperationResult<PagedResult<IReadOnlyDictionary<string, object?>>>.Success(
            new PagedResult<IReadOnlyDictionary<string, object?>>(mapped, page.Page, page.PageSize, page.TotalCount));
    }

    public async Task<OperationResult<bool>> DeleteAsync(object[] keys, CancellationToken cancellationToken = default)
    {
        var entity = await repository.FindAsync(keys, cancellationToken);
        if (entity is null)
        {
            return OperationResult<bool>.Failure(ApplicationErrors.NotFound($"{typeof(TEntity).Name} not found."));
        }

        repository.Remove(entity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return OperationResult<bool>.Success(true);
    }

    private static void ApplyPayload(TEntity entity, JsonElement payload, bool isUpdate)
    {
        if (payload.ValueKind != JsonValueKind.Object)
        {
            throw new ArgumentException("Request payload must be a JSON object.");
        }

        foreach (var property in payload.EnumerateObject())
        {
            var target = typeof(TEntity).GetProperty(property.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (target is null || !target.CanWrite || IsKeyOrNavigation(target, isUpdate))
            {
                continue;
            }

            var value = JsonSerializer.Deserialize(property.Value.GetRawText(), target.PropertyType);
            target.SetValue(entity, value);
        }
    }

    private static bool IsKeyOrNavigation(PropertyInfo property, bool isUpdate)
    {
        if (property.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) || property.Name.Equals("Id", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return property.PropertyType != typeof(string) &&
               (property.PropertyType.IsClass || property.PropertyType.IsInterface) &&
               property.PropertyType != typeof(decimal) && property.PropertyType != typeof(Guid) &&
               property.PropertyType != typeof(DateTime);
    }
}
