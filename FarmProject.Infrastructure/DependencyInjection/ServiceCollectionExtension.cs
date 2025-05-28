using FarmProject.Application;
using FarmProject.Application.FarmEventsService;
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FarmProject.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFarmInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddSqlServer<FarmDbContext>(
            connectionString,
            sqlServerOptions => sqlServerOptions.MigrationsAssembly("FarmProject.Infrastructure.Migrations")
        );

        services.AddScoped<IRabbitRepository, RabbitRepository>();
        services.AddScoped<IPairingRepository, PairingRepository>();
        services.AddScoped<IFarmEventRepository, FarmEventRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
