using Microsoft.Extensions.DependencyInjection;
using mini_crm.Application.UseCases.Customers;
using mini_crm.Application.UseCases.Shared;

namespace mini_crm.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped(typeof(GenericCrudHandler<>));
        services.AddScoped<CustomerUseCases>();
        return services;
    }
}
