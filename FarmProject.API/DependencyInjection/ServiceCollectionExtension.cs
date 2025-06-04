using FarmProject.Domain.Events;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.Events;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Application;
using FarmProject.Infrastructure.Repositories;
using FarmProject.Infrastructure;

namespace FarmProject.API.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFarmServices(this IServiceCollection services)
    {
        services.AddScoped<IBreedingRabbitService, BreedingRabbitService>()
            .AddScoped<IPairingService, PairingService>()
            .AddScoped<IFarmTaskService, FarmTaskService>()
            .AddScoped<ICageService, CageService>();
        return services;
    }

    public static IServiceCollection AddEventArchitecture(this IServiceCollection services)
    {
        services.AddScoped<IEventConsumer<BreedEvent>, BreedEventConsumer>();
        services.AddScoped<IEventConsumer<NestPrepEvent>, NestPrepEventConsumer>();
        services.AddScoped<DomainEventDispatcher>();

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

        services.AddScoped<IBreedingRabbitRepository, BreedingRabbitRepository>()
            .AddScoped<IPairingRepository, PairingRepository>()
            .AddScoped<IFarmTaskRepository, FarmTaskRepository>()
            .AddScoped<ICageRepository, CageRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}