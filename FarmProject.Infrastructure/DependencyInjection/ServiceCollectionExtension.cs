using Microsoft.Extensions.DependencyInjection;

namespace FarmProject.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFarmDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSqlServer<FarmDbContext>(
            connectionString,
            sqlServerOptions => sqlServerOptions.MigrationsAssembly("FarmProject.Infrastructure.Migrations")
        );

        return services;
    }
}
