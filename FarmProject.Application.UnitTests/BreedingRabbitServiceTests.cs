using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.UnitTests.Mocks;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FluentAssertions;

namespace FarmProject.Application.UnitTests;

public class BreedingRabbitServiceTests
{
    [Fact]
    public async Task AddBreedingRabbitToFarm_WithValidData_ReturnsBreedingRabbit()
    {
        var mockCageRepo = new MockCageRepository();
        var mockRabbitRepo = new MockBreedingRabbitRepository();

        var cage = new Cage("Test Cage") { Id = 1 };
        await mockCageRepo.AddAsync(cage);

        var mockUnitOfWork = new MockUnitOfWork(
            cageRepository: mockCageRepo,
            breedingRabbitRepository: mockRabbitRepo);

        var rabbitService = new BreedingRabbitService(mockUnitOfWork);
        var rabbitName = "Test Rabbit";

        var result = await rabbitService.AddBreedingRabbitToFarm(rabbitName, cage.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(rabbitName);
        result.Value.CageId.Should().Be(cage.Id);

        var updatedCage = await mockCageRepo.GetByIdAsync(cage.Id);
        updatedCage.BreedingRabbit.Should().NotBeNull();
        updatedCage.BreedingRabbit.Name.Should().Be(rabbitName);
    }

    [Fact]
    public async Task AddBreedingRabbitToFarm_WithOccupiedCage_ReturnsError()
    {
        var mockCageRepo = new MockCageRepository();
        var mockRabbitRepo = new MockBreedingRabbitRepository();

        var cage = new Cage("Test Cage") { Id = 1 };
        var existingRabbit = new BreedingRabbit("Existing Rabbit") { Id = 1 };
        cage.AssignBreedingRabbit(existingRabbit);
        await mockCageRepo.AddAsync(cage);

        var mockUnitOfWork = new MockUnitOfWork(
            cageRepository: mockCageRepo,
            breedingRabbitRepository: mockRabbitRepo);

        var rabbitService = new BreedingRabbitService(mockUnitOfWork);

        var result = await rabbitService.AddBreedingRabbitToFarm("New Rabbit", cage.Id);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CageErrors.Occupied);
    }

    [Fact]
    public async Task GetBreedingRabbitById_WithExistingId_ReturnsBreedingRabbit()
    {
        var mockRabbitRepo = new MockBreedingRabbitRepository();
        var rabbit = new BreedingRabbit("Test Rabbit") { Id = 1 };
        await mockRabbitRepo.AddAsync(rabbit);

        var mockUnitOfWork = new MockUnitOfWork(breedingRabbitRepository: mockRabbitRepo);
        var rabbitService = new BreedingRabbitService(mockUnitOfWork);

        var result = await rabbitService.GetBreedingRabbitById(rabbit.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(rabbit.Id);
        result.Value.Name.Should().Be(rabbit.Name);
    }

    [Fact]
    public async Task GetBreedingRabbitById_WithNonExistingId_ReturnsNotFoundError()
    {
        var mockRabbitRepo = new MockBreedingRabbitRepository();
        var mockUnitOfWork = new MockUnitOfWork(breedingRabbitRepository: mockRabbitRepo);
        var rabbitService = new BreedingRabbitService(mockUnitOfWork);

        var result = await rabbitService.GetBreedingRabbitById(999);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BreedingRabbitErrors.NotFound);
    }

    [Fact]
    public async Task GetAllAvailableBreedingRabbits_ReturnsOnlyAvailableRabbits()
    {
        var mockRabbitRepo = new MockBreedingRabbitRepository();

        var availableRabbit1 = new BreedingRabbit("Available Rabbit 1")
        {
            Id = 1,
            BreedingStatus = BreedingStatus.Available
        };
        await mockRabbitRepo.AddAsync(availableRabbit1);

        var availableRabbit2 = new BreedingRabbit("Available Rabbit 2")
        {
            Id = 2,
            BreedingStatus = BreedingStatus.Available
        };
        await mockRabbitRepo.AddAsync(availableRabbit2);

        var pregnantRabbit = new BreedingRabbit("Pregnant Rabbit")
        {
            Id = 3,
            BreedingStatus = BreedingStatus.Pregnant
        };
        await mockRabbitRepo.AddAsync(pregnantRabbit);

        var nursingRabbit = new BreedingRabbit("Nursing Rabbit")
        {
            Id = 4,
            BreedingStatus = BreedingStatus.Nursing
        };
        await mockRabbitRepo.AddAsync(nursingRabbit);

        var mockUnitOfWork = new MockUnitOfWork(breedingRabbitRepository: mockRabbitRepo);
        var rabbitService = new BreedingRabbitService(mockUnitOfWork);

        var paginatedRequest = new PaginatedRequest<BreedingRabbitFilterDto>
        {
            PageIndex = 1,
            PageSize = 50,
            Filter = new BreedingRabbitFilterDto
            {
                BreedingStatus = BreedingStatus.Available
            }
        };

        var result = await rabbitService.GetPaginatedBreedingRabbits(paginatedRequest);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(2);
        result.Value.Items.Should().Contain(r => r.Id == availableRabbit1.Id);
        result.Value.Items.Should().Contain(r => r.Id == availableRabbit2.Id);
        result.Value.Items.Should().NotContain(r => r.Id == pregnantRabbit.Id);
        result.Value.Items.Should().NotContain(r => r.Id == nursingRabbit.Id);
    }
}
