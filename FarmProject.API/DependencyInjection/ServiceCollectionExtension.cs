using FarmProject.Domain.Events;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.BreedingRabbitsService.Validators;
using FarmProject.Application.CageService;
using FarmProject.Application.Events;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Application;
using FarmProject.Infrastructure.Repositories;
using FarmProject.Infrastructure;
using FarmProject.Application.Common;
using FluentValidation;

namespace FarmProject.API.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddFarmServices(this IServiceCollection services)
    {
        services.AddScoped<ValidationHelper>();

        services.AddScoped<CageService>();
        services.AddScoped<BreedingRabbitService>();
        services.AddScoped<FarmTaskService>();
        services.AddScoped<PairingService>();

        services.AddScoped<IBreedingRabbitService>(provider => {
            var baseService = provider.GetRequiredService<BreedingRabbitService>();

            var validationHelper = provider.GetRequiredService<ValidationHelper>();
            var validatedService = new ValidationBreedingRabbitService(baseService, validationHelper);

            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingBreedingRabbitService(validatedService, loggingHelper);
        });

        services.AddScoped<ICageService>(provider => {
            var cageService = provider.GetRequiredService<CageService>();
            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingCageService(cageService, loggingHelper);
        });

        services.AddScoped<IFarmTaskService>(provider => {
            var farmTaskService = provider.GetRequiredService<FarmTaskService>();
            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingFarmTaskService(farmTaskService, loggingHelper);
        });

        services.AddScoped<IPairingService>(provider => {
            var pairingService = provider.GetRequiredService<PairingService>();
            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingPairingService(pairingService, loggingHelper);
        });

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AddBreedingRabbitParam>, AddBreedingRabbitParamValidator>();

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