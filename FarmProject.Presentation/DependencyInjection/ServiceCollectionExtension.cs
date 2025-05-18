using FarmProject.Application;
using FarmProject.Application.EventsService;
using FarmProject.Application.FarmEventsService;
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Models;
using FarmProject.Infrastructure;

namespace FarmProject.Presentation.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInMemoryRepository<T>(this IServiceCollection services)
        where T : Entity
    {
        services.AddSingleton<IRepository<T>, InMemoryRepository<T>>();
        return services;
    }

    public static IServiceCollection AddFarmServices(this IServiceCollection services)
    {
        services.AddScoped<IRabbitService, RabbitService>()
            .AddScoped<IPairingService, PairingService>()
            .AddScoped<IFarmEventService, FarmEventService>();
        return services;
    }
}
