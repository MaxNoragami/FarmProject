using FarmProject.API.Controllers;
using FarmProject.API.Dtos.Cages;
using FarmProject.API.IntegrationTests.Helpers;
using FarmProject.Application.CageService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Domain.Constants;
using FarmProject.Infrastructure;
using FarmProject.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.IntegrationTests;

public class CageControllerTests
{
    [Theory]
    [InlineData(3)]
    public async Task GetCages_ReturnsAllCages(int cagesAmount)
    {
        var (controller, factory) = await SetupTest("GetAllCagesTest",
            async seeder => await seeder.SeedCages(cagesAmount));

        using (factory)
        {
            var result = await controller.GetPaginatedCages();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var paginatedResult = Assert.IsAssignableFrom<PaginatedResult<ViewCageDto>>(okResult.Value);
            Assert.Equal(cagesAmount, paginatedResult.Items.Count);
            Assert.Equal(1, paginatedResult.PageIndex);
        }
    }

    [Theory]
    [InlineData(4)]
    public async Task GetCages_WithUnoccupiedFlag_ReturnsOnlyUnoccupiedCages(int cagesAmount)
    {
        var (controller, factory) = await SetupTest("GetUnoccupiedCagesTest",
            async seeder => await seeder.SeedCages(cagesAmount));
        using (factory)
        {
            var filter = new CageFilterDto { IsOccupied = false };
            var result = await controller.GetPaginatedCages(filter: filter);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var paginatedResult = Assert.IsAssignableFrom<PaginatedResult<ViewCageDto>>(okResult.Value);
            Assert.Equal(cagesAmount - 1, paginatedResult.Items.Count);
            Assert.All(paginatedResult.Items, cage => Assert.Null(cage.BreedingRabbitId));
        }
    }

    [Theory]
    [InlineData(4)]
    public async Task GetCage_WithValidId_ReturnsCage(int cagesAmount)
    {
        var (controller, factory) = await SetupTest("GetSingleCageTest",
            async seeder => await seeder.SeedCages(cagesAmount));
        using (factory)
        {
            var result = await controller.GetCage(cagesAmount);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var cage = Assert.IsAssignableFrom<ViewCageDto>(okResult.Value);
            Assert.Equal(cagesAmount, cage.Id);
            Assert.Equal($"Cage {cagesAmount}", cage.Name);
        }
    }

    [Theory]
    [InlineData(2)]
    public async Task GetCage_WithInvalidId_ReturnsNotFound(int cagesAmount)
    {
        var (controller, factory) = await SetupTest("GetInvalidCageTest",
            async seeder => await seeder.SeedCages(cagesAmount));
        using (factory)
        {
            var result = await controller.GetCage(cagesAmount + 1);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }

    [Theory]
    [InlineData(4)]
    [InlineData(0)]
    public async Task CreateCage_ReturnsViewCageDto(int cagesAmount)
    {
        cagesAmount = (cagesAmount < 0) ? 0 : cagesAmount;

        var (controller, factory) = await SetupTest("CreateCageTest",
            async seeder =>
            {
                await seeder.ClearDatabase();
                await seeder.SeedCages(cagesAmount);
            });

        var expectedViewCageDto = new ViewCageDto()
        {
            Id = cagesAmount + 1,
            Name = "TestCage",
            BreedingRabbitId = null,
            OffspringCount = 0,
            OffspringType = OffspringType.None
        };

        var createTestCageDto = new CreateCageDto()
        {
            Name = "TestCage"
        };

        using (factory)
        {
            var result = await controller.CreateCage(createTestCageDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedCageDto = Assert.IsAssignableFrom<ViewCageDto>(createdAtActionResult.Value);

            returnedCageDto.Should().NotBeNull();
            returnedCageDto.Should().BeEquivalentTo(expectedViewCageDto);
        }
    }

    [Theory]
    [InlineData(5)]
    public async Task UpdateCage_ToFemaleOffspringType_ReturnsViewCageDto(int cagesAmount)
    {
        var (controller, factory) = await SetupTest("UpdateCageTest",
            async seeder => await seeder.SeedCages(cagesAmount, false));

        var expectedUpdatedViewCageDto = new ViewCageDto()
        {
            Id = cagesAmount,
            Name = $"Cage {cagesAmount}",
            BreedingRabbitId = null,
            OffspringCount = 0,
            OffspringType = OffspringType.Female
        };

        var updateTestCageDto = new UpdateCageDto()
        {
            OffspringType = OffspringType.Female
        };

        using (factory)
        {
            var result = await controller.UpdateCage(cagesAmount, updateTestCageDto);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCageDto = Assert.IsAssignableFrom<ViewCageDto>(okObjectResult.Value);

            returnedCageDto.Should().NotBeNull();
            returnedCageDto.Should().BeEquivalentTo(expectedUpdatedViewCageDto);
        }
    }

    private async Task<(CageController controller, InMemoryDbContextFactory factory)> SetupTest(
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

        var cageService = new CageService(unitOfWork);
        var controller = new CageController(cageService);

        return (controller, factory);
    }
}
