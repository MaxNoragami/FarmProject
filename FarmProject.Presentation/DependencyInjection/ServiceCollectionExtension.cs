using FarmProject.Application;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Infrastructure;
using FarmProject.Infrastructure.Repositories;

namespace FarmProject.Presentation.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFarmServices(this IServiceCollection services)
    {
        services.AddScoped<IRabbitService, RabbitService>()
            .AddScoped<IPairingService, PairingService>()
            .AddScoped<IFarmTaskService, FarmTaskService>();
        return services;
    }

    public static IServiceCollection AddFarmInfrastructure(this IServiceCollection services, string connectionString)
    {
        var assemblyName = typeof(Infrastructure.Migrations.Marker)
            .Assembly
            .GetName()
            .Name;

        services.AddSqlServer<FarmDbContext>(
            connectionString,
            sqlServerOptions => sqlServerOptions.MigrationsAssembly(assemblyName)
        );

        services.AddScoped<IRabbitRepository, RabbitRepository>();
        services.AddScoped<IPairingRepository, PairingRepository>();
        services.AddScoped<IFarmTaskRepository, FarmTaskRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
