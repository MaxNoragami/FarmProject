using FarmProject.Application.RabbitsService;
using FarmProject.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace FarmProject.Presentation.DependencyInjection;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInMemoryRabbitRepo(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitRepository, RabbitRepository>();
        return services;
    }

    public static IServiceCollection AddRabbitService(this IServiceCollection services)
    {
        services.AddScoped<IRabbitService, RabbitService>();
        return services;
    }
}
