using FarmProject.Application;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Infrastructure;
using FarmProject.Infrastructure.Authentication;
using FarmProject.Infrastructure.Repositories;

namespace FarmProject.API.DependencyInjection;

public static class InfrastructureServiceCollectionExtension
{
    public static IServiceCollection AddFarmInfrastructure(
        this IServiceCollection services,
        string connectionString,
        string identityConnectionString)
    {
        var assemblyName = typeof(Infrastructure.Migrations.Marker)
            .Assembly
            .GetName()
            .Name;

        services.AddSqlServer<FarmDbContext>(
            connectionString,
            sqlServerOptions => sqlServerOptions.MigrationsAssembly(assemblyName)
        );

        services.AddSqlServer<AppIdentityDbContext>(
            identityConnectionString,
            sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(assemblyName);
                sqlServerOptions.MigrationsHistoryTable("__EFMigrationHistoryIdentity");
            }
        );

        services.AddScoped<IBreedingRabbitRepository, BreedingRabbitRepository>()
            .AddScoped<IPairingRepository, PairingRepository>()
            .AddScoped<IFarmTaskRepository, FarmTaskRepository>()
            .AddScoped<ICageRepository, CageRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
