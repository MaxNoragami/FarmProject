using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;
using FarmProject.Application.UnitTests.Mocks;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Models;
using FluentAssertions;
using PairingServiceClass = FarmProject.Application.PairingService.PairingService;

namespace FarmProject.Application.UnitTests;

public class PairingServiceTests
{
    [Fact]
    public async Task GetPairById_WithExistingId_ReturnsPair()
    {
        var mockPairingRepo = new MockPairingRepository();
        var breedingRabbit = new BreedingRabbit("Female Rabbit") { Id = 1 };
        var pair = new Pair(2, breedingRabbit, DateTime.Now) { Id = 1 };

        await mockPairingRepo.AddAsync(pair);

        var mockUnitOfWork = new MockUnitOfWork(
            pairingRepository: mockPairingRepo);

        var mockEventDispatcher = new MockDomainEventDispatcher();
        var pairingService = new PairingServiceClass(
            mockUnitOfWork,
            null!,
            mockEventDispatcher);

        var result = await pairingService.GetPairById(1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetPairById_WithNonExistingId_ReturnsNotFoundError()
    {
        var mockPairingRepo = new MockPairingRepository();
        var mockUnitOfWork = new MockUnitOfWork(
            pairingRepository: mockPairingRepo);

        var mockEventDispatcher = new MockDomainEventDispatcher();
        var pairingService = new PairingServiceClass(
            mockUnitOfWork,
            null!,
            mockEventDispatcher);

        var result = await pairingService.GetPairById(1000);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(PairErrors.NotFound);
    }

    [Fact]
    public async Task GetAllPairs_ReturnsAllPairs()
    {
        var mockPairingRepo = new MockPairingRepository();
        var breedingRabbit = new BreedingRabbit("Female Rabbit") { Id = 1 };

        var pair1 = new Pair(2, breedingRabbit, DateTime.Now) { Id = 1 };
        var pair2 = new Pair(3, breedingRabbit, DateTime.Now) { Id = 2 };

        await mockPairingRepo.AddAsync(pair1);
        await mockPairingRepo.AddAsync(pair2);

        var mockUnitOfWork = new MockUnitOfWork(
            pairingRepository: mockPairingRepo);

        var mockEventDispatcher = new MockDomainEventDispatcher();
        var pairingService = new PairingServiceClass(
            mockUnitOfWork,
            null!,
            mockEventDispatcher);

        var paginatedRequest = new PaginatedRequest<PairFilterDto>
        {
            PageIndex = 1,
            PageSize = 10
        };

        var result = await pairingService.GetPaginatedPairs(paginatedRequest);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Should().HaveCount(2);
        result.Value.Items.Should().Contain(p => p.Id == 1);
        result.Value.Items.Should().Contain(p => p.Id == 2);
    }
}
