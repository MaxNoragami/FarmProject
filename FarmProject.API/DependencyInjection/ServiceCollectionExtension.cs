using FarmProject.Application.BirthService;
using FarmProject.Application.BirthService.Validators;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.BreedingRabbitsService.Validators;
using FarmProject.Application.CageService;
using FarmProject.Application.CageService.Validators;
using FarmProject.Application.Common;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Validators;
using FarmProject.Application.Events;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Application.PairingService.Validators;
using FarmProject.Domain.Events;
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
        services.AddScoped<BirthService>();

        services.AddScoped<IBreedingRabbitService>(provider =>
        {
            var baseService = provider.GetRequiredService<BreedingRabbitService>();

            var validationHelper = provider.GetRequiredService<ValidationHelper>();
            var validatedService = new ValidationBreedingRabbitService(baseService, validationHelper);

            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingBreedingRabbitService(validatedService, loggingHelper);
        });

        services.AddScoped<ICageService>(provider =>
        {
            var baseService = provider.GetRequiredService<CageService>();

            var validationHelper = provider.GetRequiredService<ValidationHelper>();
            var validatedService = new ValidationCageService(baseService, validationHelper);

            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingCageService(validatedService, loggingHelper);
        });

        services.AddScoped<IFarmTaskService>(provider =>
        {
            var baseService = provider.GetRequiredService<FarmTaskService>();

            var validationHelper = provider.GetRequiredService<ValidationHelper>();
            var validatedService = new ValidationFarmTaskService(baseService, validationHelper);

            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingFarmTaskService(validatedService, loggingHelper);
        });

        services.AddScoped<IPairingService>(provider =>
        {
            var baseService = provider.GetRequiredService<PairingService>();

            var validationHelper = provider.GetRequiredService<ValidationHelper>();
            var validatedService = new ValidationPairingService(baseService, validationHelper);

            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingPairingService(validatedService, loggingHelper);
        });

        services.AddScoped<IBirthService>(provider =>
        {
            var baseService = provider.GetRequiredService<BirthService>();

            var validationHelper = provider.GetRequiredService<ValidationHelper>();
            var validatedService = new ValidationBirthService(baseService, validationHelper);

            var loggingHelper = provider.GetRequiredService<LoggingHelper>();
            return new LoggingBirthService(validatedService, loggingHelper);
        });

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddScoped<IValidator<AddBreedingRabbitParam>, AddBreedingRabbitParamValidator>();
        services.AddScoped<IValidator<UpdateBreedingStatusParam>, UpdateBreedingStatusParamValidator>();

        services.AddScoped<IValidator<CreateCageParam>, CreateCageParamValidator>();
        services.AddScoped<IValidator<UpdateOffspringTypeParam>, UpdateOffspringTypeParamValidator>();

        services.AddScoped<IValidator<UpdatePairingStatusParam>, UpdatePairingStatusParamValidator>();

        services.AddScoped<IValidator<PaginatedRequestParam<BreedingRabbitFilterDto>>, PaginatedRequestParamValidator<BreedingRabbitFilterDto>>();
        services.AddScoped<IValidator<PaginatedRequestParam<CageFilterDto>>, PaginatedRequestParamValidator<CageFilterDto>>();
        services.AddScoped<IValidator<PaginatedRequestParam<FarmTaskFilterDto>>, PaginatedRequestParamValidator<FarmTaskFilterDto>>();
        services.AddScoped<IValidator<PaginatedRequestParam<PairFilterDto>>, PaginatedRequestParamValidator<PairFilterDto>>();

        services.AddScoped<IValidator<RecordBirthParam>, RecordBirthParamValidator>();
        services.AddScoped<IValidator<SeparateOffspringParam>, SeparateOffspringParamValidator>();
        services.AddScoped<IValidator<WeanOffspringParam>, WeanOffspringParamValidator>();

        return services;
    }

    public static IServiceCollection AddEventArchitecture(this IServiceCollection services)
    {
        services.AddScoped<IEventConsumer<BreedEvent>, BreedEventConsumer>();
        services.AddScoped<IEventConsumer<NestPrepEvent>, NestPrepEventConsumer>();
        services.AddScoped<IEventConsumer<BirthEvent>, BirthEventConsumer>();
        services.AddScoped<DomainEventDispatcher>();

        return services;
    }
}