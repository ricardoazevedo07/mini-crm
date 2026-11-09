using System.Collections;
using System.Reflection;

namespace mini_crm.Application.UseCases.Shared;

public static class EntityResponseMapper
{
    public static IReadOnlyDictionary<string, object?> Map<TEntity>(TEntity entity)
    {
        return typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(property => IsScalar(property.PropertyType))
            .ToDictionary(property => property.Name, property => property.GetValue(entity));
    }

    private static bool IsScalar(Type type) =>
        type.IsPrimitive || type.IsEnum || type == typeof(string) || type == typeof(decimal) ||
        type == typeof(Guid) || type == typeof(DateTime) || type == typeof(DateTime?) ||
        Nullable.GetUnderlyingType(type) is not null || !typeof(IEnumerable).IsAssignableFrom(type);
}
