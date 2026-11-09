using Microsoft.Extensions.DependencyInjection;
using mini_crm.Domain.IRepositories;
using mini_crm.Infrastructure.Repositories;

namespace mini_crm.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped(typeof(IPagedRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<IEntityReferenceRepository, EntityReferenceRepository>();
        services.AddScoped<IEntidadeComercialRepository, EntidadeComercialRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IInfluencerRepository, InfluencerRepository>();
        services.AddScoped<IRedeSocialRepository, RedeSocialRepository>();
        services.AddScoped<IInfluencerRedeSocialRepository, InfluencerRedeSocialRepository>();
        services.AddScoped<IAgenciaRepository, AgenciaRepository>();
        services.AddScoped<IAgenteRepository, AgenteRepository>();
        services.AddScoped<IAgenciaAgenteRepository, AgenciaAgenteRepository>();
        services.AddScoped<IIntermediarioRepository, IntermediarioRepository>();
        services.AddScoped<IBancoRepository, BancoRepository>();
        services.AddScoped<IContaBancariaRepository, ContaBancariaRepository>();
        services.AddScoped<IInfluencerContaBancariaRepository, InfluencerContaBancariaRepository>();
        services.AddScoped<IContratoRepository, ContratoRepository>();
        services.AddScoped<IDespesaRepository, DespesaRepository>();
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();
        services.AddScoped<INotaFiscalRepository, NotaFiscalRepository>();
        services.AddScoped<IFormatoNotaFiscalRepository, FormatoNotaFiscalRepository>();
        services.AddScoped<IStatusNotaFiscalRepository, StatusNotaFiscalRepository>();
        services.AddScoped<IAgendamentoNotaFiscalRepository, AgendamentoNotaFiscalRepository>();
        return services;
    }
}
