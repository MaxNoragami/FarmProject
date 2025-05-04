using FarmProject.Application.Rabbits;
using Microsoft.Extensions.DependencyInjection;

namespace FarmProject.Infrastructure;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRabbitRepo(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitRepository, RabbitRepository>();
        return services;
    }
}
