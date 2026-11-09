using Microsoft.Extensions.DependencyInjection;
using mini_crm.Application.UseCases.Shared;

namespace mini_crm.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(GenericCrudHandler<>));
        return services;
    }
}
