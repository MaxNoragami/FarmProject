using FarmProject.API.Controllers;
using FarmProject.API.Dtos.BreedingRabbits;
using FarmProject.API.IntegrationTests.Helpers;
using FarmProject.Application.BirthService;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.CageService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Events;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Services;
using FarmProject.Infrastructure;
using FarmProject.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FarmProject.API.IntegrationTests;

public class BreedingRabbitControllerTests
{
    [Theory]
    [InlineData(3)]
    [InlineData(5)]
    public async Task GetBreedingRabbits_ReturnsAllBreedingRabbits(int breedingRabbitsAmount)
    {
        var (controller, factory) = await SetupTest("GetAllBreedingRabbitsTest",
            async seeder => await seeder.SeedCagesWithRabbits(breedingRabbitsAmount));

        using (factory)
        {
            var result = await controller.GetPaginatedBreedingRabbits();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var paginatedResult = Assert.IsAssignableFrom<PaginatedResult<ViewBreedingRabbitDto>>(okResult.Value);
            Assert.Equal(breedingRabbitsAmount, paginatedResult.Items.Count);
        }
    }

    [Theory]
    [InlineData(2)]
    [InlineData(4)]
    public async Task GetBreedingRabbit_WithValidId_ReturnsBreedingRabbit(int breedingRabbitsAmount)
    {
        var (controller, factory) = await SetupTest("GetSingleBreedingRabbitTest",
            async seeder => await seeder.SeedCagesWithRabbits(breedingRabbitsAmount));
        var expectedBreedingRabbitId = breedingRabbitsAmount;

        using (factory)
        {
            var result = await controller.GetBreedingRabbit(expectedBreedingRabbitId);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var breedingRabbit = Assert.IsAssignableFrom<ViewBreedingRabbitDto>(okResult.Value);
            Assert.Equal(expectedBreedingRabbitId, breedingRabbit.Id);
            Assert.Equal($"Breeding Rabbit {expectedBreedingRabbitId}", breedingRabbit.Name);
        }
    }

    [Theory]
    [InlineData(3)]
    [InlineData(1)]
    public async Task GetBreedingRabbit_WithInvalidId_ReturnsNotFound(int breedingRabbitsAmount)
    {
        var (controller, factory) = await SetupTest("GetInvalidBreedingRabbitTest",
            async seeder => await seeder.SeedCagesWithRabbits(breedingRabbitsAmount));
        var expectedBreedingRabbitId = breedingRabbitsAmount + 1;

        using (factory)
        {
            var result = await controller.GetBreedingRabbit(expectedBreedingRabbitId);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }

    [Theory]
    [InlineData(4, 3)]
    [InlineData(8, 8)]
    public async Task CreateBreedingRabbit_ReturnsViewBreedingRabbitDto(int cagesAmount, int targetCageId)
    {
        if (targetCageId > cagesAmount || targetCageId < 1)
            targetCageId = cagesAmount - 1;

        var (controller, factory) = await SetupTest("CreateBreedingRabbitTest",
            async seeder => 
                { 
                    await seeder.ClearDatabase(); 
                    await seeder.SeedCages(cagesAmount, false); 
                });

        var expectedViewBreedingRabbitDto = new ViewBreedingRabbitDto()
        {
            Id = 1,
            Name = $"Breeding Rabbit 1",
            CageId = targetCageId,
            BreedingStatus = BreedingStatus.Available
        };

        var createTestBreedingRabbitDto = new CreateBreedingRabbitDto()
        {
            Name = $"Breeding Rabbit 1",
            CageId = targetCageId
        };

        using (factory)
        {
            var result = await controller.CreateBreedingRabbit(createTestBreedingRabbitDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedBreedingRabbitDto = Assert.IsAssignableFrom<ViewBreedingRabbitDto>(createdAtActionResult.Value);

            returnedBreedingRabbitDto.Should().NotBeNull();
            returnedBreedingRabbitDto.Should().BeEquivalentTo(expectedViewBreedingRabbitDto);
        }
    }

    [Theory]
    [InlineData(4, 3)]
    [InlineData(10, 1)]
    [InlineData(2, 2)]
    public async Task UpdateBreedingRabbit_MoveRabbitToAnotherCage_ReturnsViewBreedingRabbitDto(
        int cagesAmount, 
        int targetCage)
    {
        if (targetCage >= cagesAmount || targetCage < 1)
            targetCage = cagesAmount - 1;

        var (controller, factory) = await SetupTest("UpdateBreedingRabbitTest",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedCages(cagesAmount);
            });

        var expectedViewBreedingRabbitDto = new ViewBreedingRabbitDto()
        {
            Id = cagesAmount,
            Name = $"Breeding Rabbit {cagesAmount}",
            CageId = targetCage,
            BreedingStatus = BreedingStatus.Available
        };

        var updateTestBreedingRabbitDto = new UpdateBreedingRabbitDto()
        {
            CageId = targetCage
        };

        using (factory)
        {
            var result = await controller.UpdateBreedingRabbit(cagesAmount, updateTestBreedingRabbitDto);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedBreedingRabbitDto = Assert.IsAssignableFrom<ViewBreedingRabbitDto>(okObjectResult.Value);

            returnedBreedingRabbitDto.Should().NotBeNull();
            returnedBreedingRabbitDto.Should().BeEquivalentTo(expectedViewBreedingRabbitDto);
        }
    }

    private async Task<(BreedingRabbitController controller, InMemoryDbContextFactory factory)> SetupTest(
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
        var customerRepository = new CustomerRepository(dbContext);
        var unitOfWork = new UnitOfWork(
            dbContext,
            breedingRabbitRepository,
        pairingRepository,
            farmTaskRepository,
            cageRepository,
            customerRepository
        );

        var services = new ServiceCollection();

        var serviceProvider = services.BuildServiceProvider();
        var domainEventDispatcher = new DomainEventDispatcher(serviceProvider);
        var birthService = new BirthService(unitOfWork, domainEventDispatcher, new BirthDomainService());

        var breedingRabbitService = new BreedingRabbitService(unitOfWork);
        var cageService = new CageService(unitOfWork, 60);
        var controller = new BreedingRabbitController(breedingRabbitService, cageService, birthService);

        return (controller, factory);
    }
}
