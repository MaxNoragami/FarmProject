using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.Common.Models;
using FarmProject.Application.UnitTests.Mocks;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FluentAssertions;

namespace FarmProject.Application.UnitTests;

public class CageServiceTests
{
    [Fact]
    public async Task CreateCage_WithValidName_ReturnsCreatedCage()
    {
        var mockCageRepo = new MockCageRepository();
        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);
        var cageName = "Test Cage";

        var result = await cageService.CreateCage(cageName);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(cageName);
        result.Value.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetAllCages_WithExistingCages_ReturnsAllCages()
    {
        var mockCageRepo = new MockCageRepository();
        await mockCageRepo.AddAsync(new Cage("Cage 1"));
        await mockCageRepo.AddAsync(new Cage("Cage 2"));
        await mockCageRepo.AddAsync(new Cage("Cage 3"));

        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var paginatedRequest = new PaginatedRequest<CageFilterDto>
        {
            PageIndex = 1,
            PageSize = 10
        };

        var result = await cageService.GetPaginatedCages(paginatedRequest);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetAllCages_WithNoCages_ReturnsEmptyList()
    {
        var mockCageRepo = new MockCageRepository();
        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var paginatedRequest = new PaginatedRequest<CageFilterDto>
        {
            PageIndex = 1,
            PageSize = 10
        };

        var result = await cageService.GetPaginatedCages(paginatedRequest);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().BeEmpty();
    }

    [Fact]
    public async Task GetCageById_WithExistingId_ReturnsCage()
    {
        var mockCageRepo = new MockCageRepository();
        var cage = await mockCageRepo.AddAsync(new Cage("Test Cage"));

        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.GetCageById(cage.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(cage.Id);
        result.Value.Name.Should().Be(cage.Name);
    }

    [Fact]
    public async Task GetCageById_WithNonExistingId_ReturnsNotFound()
    {
        var mockCageRepo = new MockCageRepository();
        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.GetCageById(999);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CageErrors.NotFound);
    }

    [Fact]
    public async Task GetUnoccupiedCages_WithUnoccupiedCages_ReturnsUnoccupiedCages()
    {
        var mockCageRepo = new MockCageRepository();
        var cage1 = await mockCageRepo.AddAsync(new Cage("Cage 1"));

        var cage2 = await mockCageRepo.AddAsync(new Cage("Cage 2"));
        cage2.AssignBreedingRabbit(new BreedingRabbit("Test Rabbit"));

        var cage3 = await mockCageRepo.AddAsync(new Cage("Cage 3"));

        var cage4 = await mockCageRepo.AddAsync(new Cage("Cage 4"));
        cage4.AddOffspring(5);

        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.GetUnoccupiedCages();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().Contain(c => c.Id == cage1.Id);
        result.Value.Should().Contain(c => c.Id == cage3.Id);
        result.Value.Should().NotContain(c => c.Id == cage2.Id);
        result.Value.Should().NotContain(c => c.Id == cage4.Id);
    }

    [Fact]
    public async Task GetUnoccupiedCages_WithNoUnoccupiedCages_ReturnsEmptyList()
    {
        var mockCageRepo = new MockCageRepository();
        var cage1 = await mockCageRepo.AddAsync(new Cage("Cage 1"));
        cage1.AssignBreedingRabbit(new BreedingRabbit("Rabbit 1"));

        var cage2 = await mockCageRepo.AddAsync(new Cage("Cage 2"));
        cage2.AddOffspring(3);

        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.GetUnoccupiedCages();

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    [Fact]
    public async Task UpdateOffspringType_WithValidInput_UpdatesType()
    {
        var mockCageRepo = new MockCageRepository();
        var cage = await mockCageRepo.AddAsync(new Cage("Test Cage"));

        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.UpdateOffspringType(cage.Id, OffspringType.Female);

        result.IsSuccess.Should().BeTrue();
        result.Value.OffspringType.Should().Be(OffspringType.Female);
    }

    [Fact]
    public async Task UpdateOffspringType_WithNonExistingId_ReturnsNotFound()
    {
        var mockCageRepo = new MockCageRepository();
        var mockUnitOfWork = new MockUnitOfWork(cageRepository: mockCageRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.UpdateOffspringType(999, OffspringType.Male);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CageErrors.NotFound);
    }

    [Fact]
    public async Task MoveBreedingRabbitToCage_FromOneCageToAnother_MovesRabbit()
    {
        var mockCageRepo = new MockCageRepository();
        var mockRabbitRepo = new MockBreedingRabbitRepository();

        var sourceCage = await mockCageRepo.AddAsync(new Cage("Source Cage"));
        var destinationCage = await mockCageRepo.AddAsync(new Cage("Destination Cage"));
        var rabbit = await mockRabbitRepo.AddAsync(new BreedingRabbit("Test Rabbit"));

        sourceCage.AssignBreedingRabbit(rabbit);
        rabbit.CageId = sourceCage.Id;
        await mockCageRepo.UpdateAsync(sourceCage);

        var mockUnitOfWork = new MockUnitOfWork(
            cageRepository: mockCageRepo,
            breedingRabbitRepository: mockRabbitRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.MoveBreedingRabbitToCage(rabbit.Id, destinationCage.Id);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(destinationCage.Id);
        result.Value.BreedingRabbit.Should().NotBeNull();
        result.Value.BreedingRabbit.Id.Should().Be(rabbit.Id);

        var sourceResult = await mockCageRepo.GetByIdAsync(sourceCage.Id);
        sourceResult.BreedingRabbit.Should().BeNull();

        mockUnitOfWork.TransactionStarted.Should().BeTrue();
        mockUnitOfWork.TransactionCommitted.Should().BeTrue();
    }

    [Fact]
    public async Task MoveBreedingRabbitToCage_ToOccupiedCage_ReturnsError()
    {
        var mockCageRepo = new MockCageRepository();
        var mockRabbitRepo = new MockBreedingRabbitRepository();

        var destinationCage = await mockCageRepo.AddAsync(new Cage("Destination Cage"));
        var existingRabbit = await mockRabbitRepo.AddAsync(new BreedingRabbit("Existing Rabbit"));
        destinationCage.AssignBreedingRabbit(existingRabbit);

        var rabbit = await mockRabbitRepo.AddAsync(new BreedingRabbit("Test Rabbit"));

        var mockUnitOfWork = new MockUnitOfWork(
            cageRepository: mockCageRepo,
            breedingRabbitRepository: mockRabbitRepo);
        var cageService = new CageService.CageService(mockUnitOfWork);

        var result = await cageService.MoveBreedingRabbitToCage(rabbit.Id, destinationCage.Id);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(CageErrors.Occupied);
        mockUnitOfWork.TransactionStarted.Should().BeTrue();
        mockUnitOfWork.TransactionRolledBack.Should().BeTrue();
    }
}
