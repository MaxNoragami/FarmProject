using FarmProject.API.Controllers;
using FarmProject.API.Dtos.Cages;
using FarmProject.API.IntegrationTests.Helpers;
using FarmProject.Application.CageService;
using FarmProject.Domain.Constants;
using FarmProject.Infrastructure;
using FarmProject.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.IntegrationTests;

public class CageControllerTests
{
    [Fact]
    public async Task GetCages_ReturnsAllCages()
    {
        var (controller, factory) = await SetupTest("GetAllCagesTest",
            async seeder => await seeder.SeedCages(5));

        using (factory)
        {
            var result = await controller.GetCages();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var cages = Assert.IsAssignableFrom<List<ViewCageDto>>(okResult.Value);
            Assert.Equal(5, cages.Count);
        }
    }

    [Fact]
    public async Task GetCages_WithUnoccupiedFlag_ReturnsOnlyUnoccupiedCages()
    {
        var (controller, factory) = await SetupTest("GetUnoccupiedCagesTest");
        using (factory)
        {
            var result = await controller.GetCages(unoccupiedCages: true);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var cages = Assert.IsAssignableFrom<List<ViewCageDto>>(okResult.Value);
            Assert.Equal(2, cages.Count);
            Assert.All(cages, cage => Assert.Null(cage.BreedingRabbitId));
        }
    }

    [Fact]
    public async Task GetCage_WithValidId_ReturnsCage()
    {
        var (controller, factory) = await SetupTest("GetSingleCageTest");
        using (factory)
        {
            var result = await controller.GetCage(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var cage = Assert.IsAssignableFrom<ViewCageDto>(okResult.Value);
            Assert.Equal(1, cage.Id);
            Assert.Equal("Cage 1", cage.Name);
        }
    }

    [Fact]
    public async Task GetCage_WithInvalidId_ReturnsNotFound()
    {
        var (controller, factory) = await SetupTest("GetInvalidCageTest");
        using (factory)
        {
            var result = await controller.GetCage(100);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }

    [Fact]
    public async Task CreateCage_ReturnsViewCageDto()
    {
        var (controller, factory) = await SetupTest("CreateCageTest",
            async seeder => await seeder.ClearDatabase());

        var expectedViewCageDto = new ViewCageDto()
        {
            Id = 1,
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

    [Fact]
    public async Task UpdateCage_ToFemaleOffspringType_ReturnsViewCageDto()
    {
        var (controller, factory) = await SetupTest("UpdateCageTest",
            async seeder => await seeder.SeedCages(1, false));

        var expectedUpdatedViewCageDto = new ViewCageDto()
        {
            Id = 1,
            Name = "Cage 1",
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
            var result = await controller.UpdateCage(1, updateTestCageDto);

            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCageDto = Assert.IsAssignableFrom<ViewCageDto>(okObjectResult.Value);

            returnedCageDto.Should().NotBeNull();
            returnedCageDto.Should().BeEquivalentTo(expectedUpdatedViewCageDto);
        }
    }

    private async Task<(CageController controller, InMemoryDbContextFactory factory)> SetupTest(
        string databaseName,
        Func<TestDataSeeder, Task>? setupAction = null)
    {
        var factory = new InMemoryDbContextFactory(databaseName);
        var dbContext = factory.GetContext();

        var seeder = new TestDataSeeder(dbContext);

        if (setupAction == null)
            await seeder.SeedCages(3);
        else
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
