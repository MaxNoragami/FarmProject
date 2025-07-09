using FarmProject.API.Controllers;
using FarmProject.API.Dtos.FarmTasks;
using FarmProject.API.Dtos.Pairs;
using FarmProject.API.IntegrationTests.Helpers;
using FarmProject.Application;
using FarmProject.Application.BirthService;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Events;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.PairingService;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Events;
using FarmProject.Domain.Services;
using FarmProject.Infrastructure;
using FarmProject.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FarmProject.API.IntegrationTests;

public class FarmTaskControllerTests
{
    [Fact]
    public async Task GetFarmTaskByDate_DateWithTasks_ReturnsFarmTasks()
    {
        var (farmTaskController, pairController, factory) = await SetupTest("GetFarmTaskByDate",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedPairs(1);
            });

        var updatePairDto = new UpdatePairDto()
        {
            PairingStatus = PairingStatus.Successful
        };

        using (factory)
        {
            var pairUpdateResult = await pairController.UpdatePair(1, updatePairDto);
            var okPairUpdateResult = Assert.IsType<OkObjectResult>(pairUpdateResult.Result);
            var returnedPairDto = Assert.IsAssignableFrom<ViewPairDto>(okPairUpdateResult.Value);
            
            returnedPairDto.PairingStatus.Should().Be(PairingStatus.Successful);

            var endDate = returnedPairDto.EndDate!.Value;
            var expectedDueDate = endDate.AddMonths(1).AddDays(-3).Date;

            var filter = new FarmTaskFilterDto { DueOn = expectedDueDate.ToString("yyyy-MM-dd")};
            var farmTaskResult = await farmTaskController.GetPaginatedFarmTasks(filter: filter);

            var okFarmTaskResult = Assert.IsType<OkObjectResult>(farmTaskResult.Result);
            var returnedFarmTaskDto = Assert.IsAssignableFrom<PaginatedResult<ViewFarmTaskDto>>(okFarmTaskResult.Value);

            returnedFarmTaskDto.Items.Should().NotBeEmpty();
            returnedFarmTaskDto.Items.FirstOrDefault()?.DueOn.Date.Should().Be(expectedDueDate.Date);
        }
    }

    [Fact]
    public async Task MarkTaskCompleted_ReturnsFarmTask()
    {
        var (farmTaskController, pairController, factory) = await SetupTest("MarkTaskCompleted",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedPairs(1);
            });

        var updatePairDto = new UpdatePairDto()
        {
            PairingStatus = PairingStatus.Successful
        };

        using (factory)
        {
            var pairUpdateResult = await pairController.UpdatePair(1, updatePairDto);
            var okPairUpdateResult = Assert.IsType<OkObjectResult>(pairUpdateResult.Result);
            var returnedPairDto = Assert.IsAssignableFrom<ViewPairDto>(okPairUpdateResult.Value);

            returnedPairDto.PairingStatus.Should().Be(PairingStatus.Successful);

            var endDate = returnedPairDto.EndDate!.Value;
            var expectedDueDate = endDate.AddMonths(1).AddDays(-3).Date;

            var farmTaskResult = await farmTaskController.MarkTaskCompleted(1, null);
            var okFarmTaskResult = Assert.IsType<OkObjectResult>(farmTaskResult.Result);
            var returnedFarmTaskDto = Assert.IsAssignableFrom<ViewFarmTaskDto>(okFarmTaskResult.Value);

            returnedFarmTaskDto.DueOn.Date.Should().Be(expectedDueDate.Date);
            returnedFarmTaskDto.IsCompleted.Should().Be(true);
        }
    }

    private async Task<(FarmTaskController farmTaskController, PairController pairController, InMemoryDbContextFactory factory)> SetupTest(
        string databaseName,
        Func<TestDataSeeder, Task> setupAction)
    {
        var factory = new InMemoryDbContextFactory(databaseName);
        var dbContext = factory.GetContext();

        var seeder = new TestDataSeeder(dbContext);

        await setupAction(seeder);

        var cageRepository = new CageRepository(dbContext);
        var breedingRabbitRepository = new BreedingRabbitRepository(dbContext);
        var farmTaskRepository = new FarmTaskRepository(dbContext);
        var pairingRepository = new PairingRepository(dbContext);
        var unitOfWork = new UnitOfWork(
            dbContext,
            breedingRabbitRepository,
            pairingRepository,
            farmTaskRepository,
            cageRepository
        );

        var services = new ServiceCollection();

        services.AddSingleton<IPairingRepository>(pairingRepository);
        services.AddSingleton<IBreedingRabbitRepository>(breedingRabbitRepository);
        services.AddSingleton<IFarmTaskRepository>(farmTaskRepository);
        services.AddSingleton<ICageRepository>(cageRepository);

        services.AddScoped<IEventConsumer<BreedEvent>, BreedEventConsumer>();
        services.AddScoped<IEventConsumer<NestPrepEvent>, NestPrepEventConsumer>();

        var breedingRabbitService = new BreedingRabbitService(unitOfWork);
        services.AddSingleton<IBreedingRabbitService>(breedingRabbitService);
        services.AddSingleton<IUnitOfWork>(unitOfWork);

        var serviceProvider = services.BuildServiceProvider();

        var domainEventDispatcher = new DomainEventDispatcher(serviceProvider);
        var pairingService = new PairingService(unitOfWork, breedingRabbitService, domainEventDispatcher);

        var birthService = new BirthService(unitOfWork, domainEventDispatcher, new BirthDomainService());

        var farmTaskService = new FarmTaskService(unitOfWork, birthService);

        var pairController = new PairController(pairingService);
        var farmTaskController = new FarmTaskController(farmTaskService);

        return (farmTaskController, pairController, factory);
    }
}
