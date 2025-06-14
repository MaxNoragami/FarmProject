
using FarmProject.Application.Events;
using FarmProject.Application.PairingService;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;
using FluentAssertions;
using FarmProject.Domain.Specifications;
using FarmProject.Application.Common.Models;
using FarmProject.Application.Common.Models.Dtos;

namespace FarmProject.Domain.UnitTests.ListnerTest;

public class BreedEventTest
{
    [Fact]
    public async Task Consume_BreedEvent_CreatesPair()
    {
        IPairingRepository pairingRepository = new InMemoryPairingRepo();
        IBreedingRabbitRepository breedingRabbitRepository = new InMemoryBreedingRabbitRepo();

        var femaleBreedingRabbit = new BreedingRabbit("Female")
        {
            Id = 1
        };
        var maleRabbitId = 2;

        await breedingRabbitRepository.AddAsync(femaleBreedingRabbit);

        var eventConsumer = new BreedEventConsumer(pairingRepository, breedingRabbitRepository);
        var startDate = DateTime.Now;
        var breedEvent = new BreedEvent()
        {
            BreedingRabbitIds = [1, 2],
            StartDate = startDate
        };

        var consumeResult = await eventConsumer.ConsumeAsync(breedEvent);

        var paginationRequest = new PaginatedRequest<PairFilterDto>
        {
            PageIndex = 1,
            PageSize = 10
        };
        var paginatedResult = await pairingRepository.GetPaginatedAsync(paginationRequest);
        var pairs = paginatedResult.Items.ToList();

        Assert.Single(pairs);
        consumeResult.IsSuccess.Should().BeTrue();
        pairs[0].FemaleRabbit.Should().BeEquivalentTo(femaleBreedingRabbit);
        pairs[0].MaleRabbitId.Should().Be(maleRabbitId);

    }
}

internal class InMemoryBreedingRabbitRepo : IBreedingRabbitRepository
{
    private List<BreedingRabbit> _breedingRabbits = [];

    public Task<BreedingRabbit> AddAsync(BreedingRabbit breedingRabbit)
    {
        _breedingRabbits.Add(breedingRabbit);
        return Task.FromResult(breedingRabbit);
    }

    public Task<List<BreedingRabbit>> FindAsync(ISpecification<BreedingRabbit> specification)
    {
        throw new NotImplementedException();
    }

    public Task<BreedingRabbit?> GetByIdAsync(int breedingRabbitId)
    {
        return Task.FromResult(_breedingRabbits.FirstOrDefault(r => r.Id == breedingRabbitId));
    }

    public Task<PaginatedResult<BreedingRabbit>> GetPaginatedAsync(PaginatedRequest<BreedingRabbitFilterDto> request)
    {
        var items = _breedingRabbits.Skip((request.PageIndex - 1) * request.PageSize)
                                   .Take(request.PageSize)
                                   .ToList();

        var totalPages = (int)Math.Ceiling(_breedingRabbits.Count / (double)request.PageSize);
        var result = new PaginatedResult<BreedingRabbit>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return Task.FromResult(result);
    }

    public Task<bool> IsNameUsedAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(BreedingRabbit breedingRabbit)
    {
        throw new NotImplementedException();
    }

    public Task<BreedingRabbit> UpdateAsync(BreedingRabbit breedingRabbit)
    {
        throw new NotImplementedException();
    }
}

internal class InMemoryPairingRepo : IPairingRepository
{

    private List<Pair> _pairs = [];

    public Task<Pair> AddAsync(Pair pair)
    {
        _pairs.Add(pair);
        return Task.FromResult(pair);
    }

    public Task<Pair?> GetByIdAsync(int pairId)
    {
        throw new NotImplementedException();
    }

    public Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId1, int breedingRabbitId2)
    {
        throw new NotImplementedException();
    }

    public Task<PaginatedResult<Pair>> GetPaginatedAsync(PaginatedRequest<PairFilterDto> request)
    {
        var items = _pairs.Skip((request.PageIndex - 1) * request.PageSize)
                         .Take(request.PageSize)
                         .ToList();

        var totalPages = (int)Math.Ceiling(_pairs.Count / (double)request.PageSize);
        var result = new PaginatedResult<Pair>(
            request.PageIndex,
            request.PageSize,
            totalPages,
            items
        );

        return Task.FromResult(result);
    }

    public Task RemoveAsync(Pair pair)
    {
        throw new NotImplementedException();
    }

    public Task<Pair> UpdateAsync(Pair pair)
    {
        throw new NotImplementedException();
    }
}
