
using FarmProject.Application.Events;
using FarmProject.Application.PairingService;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;
using FluentAssertions;
using FarmProject.Domain.Specifications;

namespace FarmProject.Domain.UnitTests.ListnerTest;

public class BreedEventTest
{
    [Fact]
    public async Task SavePairOnBreedEventAsync()
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

        var pairingResult = await pairingRepository.GetAllAsync();

        Assert.Single(pairingResult);
        consumeResult.IsSuccess.Should().BeTrue();
        pairingResult[0].FemaleRabbit.Should().BeEquivalentTo(femaleBreedingRabbit);
        pairingResult[0].MaleRabbitId.Should().Be(maleRabbitId);

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

    public Task<List<BreedingRabbit>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<BreedingRabbit?> GetByIdAsync(int breedingRabbitId)
    {
        return Task.FromResult(_breedingRabbits.FirstOrDefault(r => r.Id == breedingRabbitId));
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

    public Task<List<Pair>> GetAllAsync()
    {
        return Task.FromResult(_pairs);
    }

    public Task<Pair?> GetByIdAsync(int pairId)
    {
        throw new NotImplementedException();
    }

    public Task<Pair?> GetMostRecentPairByBreedingRabbitIdsAsync(int breedingRabbitId1, int breedingRabbitId2)
    {
        throw new NotImplementedException();
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
