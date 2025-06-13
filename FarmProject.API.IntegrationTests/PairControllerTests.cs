using FarmProject.API.Controllers;
using FarmProject.API.IntegrationTests.Helpers;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Infrastructure.Repositories;
using FarmProject.Infrastructure;
using FarmProject.Application.PairingService;
using FarmProject.Application.Events;
using FarmProject.Application;
using FarmProject.Domain.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using FarmProject.API.Dtos.Pairs;
using FarmProject.Domain.Constants;
using FluentAssertions;
using FarmProject.Application.CageService;
using FarmProject.Application.FarmTaskService;
using FarmProject.Application.Common.Models;

namespace FarmProject.API.IntegrationTests;

public class PairControllerTests
{
    [Theory]
    [InlineData(3)]
    public async Task GetPairs_ReturnsAllPairs(int pairsAmount)
    {
        var (controller, factory) = await SetupTest("GetAllPairsTest",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedPairs(pairsAmount);
            });

        using (factory)
        {
            var result = await controller.GetPaginatedPairs();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var paginatedResult = Assert.IsAssignableFrom<PaginatedResult<ViewPairDto>>(okResult.Value);
            Assert.Equal(pairsAmount, paginatedResult.Items.Count);
        }
    }

    [Theory]
    [InlineData(4)]
    public async Task GetPair_WithValidId_ReturnsPair(int pairsAmount)
    {
        var (controller, factory) = await SetupTest("GetSinglePairTest",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedPairs(pairsAmount);
            });

        using (factory)
        {
            var result = await controller.GetPair(pairsAmount);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var pair = Assert.IsAssignableFrom<ViewPairDto>(okResult.Value);
            Assert.Equal(pairsAmount, pair.Id);
        }
    }

    [Theory]
    [InlineData(4)]
    public async Task GetPair_WithInvalidId_ReturnsNotFound(int pairsAmount)
    {
        var (controller, factory) = await SetupTest("GetInvalidPairTest",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedPairs(pairsAmount);
            });

        using (factory)
        {
            var result = await controller.GetPair(pairsAmount + 1);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }

    [Theory]
    [InlineData(4)]
    [InlineData(8)]
    public async Task CreatePair_ReturnsViewPairDto(int pairsAmount)
    {
        var newIds = pairsAmount + 1;

        var (controller, factory) = await SetupTest("CreatePairTest",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedPairs(pairsAmount);
                await seeder.SeedCageWithAssignedRabbit();
            });

        var createTestPairDto = new CreatePairDto()
        {
            FemaleRabbitId = newIds,
            MaleRabbitId = newIds + 1000
        };

        using (factory)
        {
            var result = await controller.CreatePair(createTestPairDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedPairDto = Assert.IsAssignableFrom<ViewPairDto>(createdAtActionResult.Value);

            returnedPairDto.Should().NotBeNull();
            returnedPairDto.FemaleRabbitId.Should().Be(newIds);
            returnedPairDto.MaleRabbitId.Should().Be(newIds + 1000);
            returnedPairDto.PairingStatus.Should().Be(PairingStatus.Active);
        }
    }

    [Theory]
    [InlineData(4, 3)]
    [InlineData(8, 4)]
    public async Task UpdatePair_ValidSuccessfulPairingStatus_ReturnsViewPairDto(int pairsAmount, int targetPairId)
    {
        if (targetPairId > pairsAmount || targetPairId < 1)
            targetPairId = pairsAmount - 1;

        var (controller, factory) = await SetupTest("UpdatePairTest",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedPairs(pairsAmount);
            });

        var updatePairDto = new UpdatePairDto()
        {
            PairingStatus = PairingStatus.Successful
        };

        using (factory)
        {
            var result = await controller.UpdatePair(targetPairId, updatePairDto);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedPairDto = Assert.IsAssignableFrom<ViewPairDto>(okObjectResult.Value);

            returnedPairDto.Should().NotBeNull();
            returnedPairDto.FemaleRabbitId.Should().Be(targetPairId );
            returnedPairDto.MaleRabbitId.Should().Be(targetPairId + 1000);
            returnedPairDto.PairingStatus.Should().Be(PairingStatus.Successful);
        }
    }

    private async Task<(PairController controller, InMemoryDbContextFactory factory)> SetupTest(
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
        var controller = new PairController(pairingService);

        return (controller, factory);
    }

}
