
using FarmProject.Application.Events;
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;
using FluentAssertions;

namespace FarmProject.Domain.UnitTests.ListnerTest;

public class BreedEventTest
{
    [Fact]
    public async Task SavePairOnBreedEventAsync()
    {
        IPairingRepository pairingRepository = new InMemoryPairingRepo();
        IRabbitRepository rabbitRepository = new InMemoryRabbitRepo();

        var femaleRabbit = new Rabbit("Female", Gender.Female)
        {
            Id = 1
        };
        var maleRabbit = new Rabbit("Male", Gender.Male)
        { 
            Id = 2
        };

        await rabbitRepository.AddAsync(femaleRabbit);
        await rabbitRepository.AddAsync(maleRabbit);

        var eventConsumer = new BreedEventConsumer(pairingRepository, rabbitRepository);
        var startDate = DateTime.Now;
        var breedEvent = new BreedEvent()
        {
            RabbitIds = [1, 2],
            StartDate = startDate
        };

        var consumeResult = await eventConsumer.ConsumeAsync(breedEvent);

        var pairingResult = await pairingRepository.GetAllAsync();

        Assert.Single(pairingResult);
        consumeResult.IsSuccess.Should().BeTrue();
        pairingResult[0].FemaleRabbit.Should().BeEquivalentTo(femaleRabbit);
        pairingResult[0].MaleRabbit.Should().BeEquivalentTo(maleRabbit);

    }
}

internal class InMemoryRabbitRepo : IRabbitRepository
{
    private List<Rabbit> _rabbits = [];

    public Task<Rabbit> AddAsync(Rabbit rabbit)
    {
        _rabbits.Add(rabbit);
        return Task.FromResult(rabbit);
    }

    public Task<List<Rabbit>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Rabbit?> GetByIdAsync(int rabbitId)
    {
        return Task.FromResult(_rabbits.FirstOrDefault(r => r.Id == rabbitId));
    }

    public Task RemoveAsync(Rabbit rabbit)
    {
        throw new NotImplementedException();
    }

    public Task<Rabbit> UpdateAsync(Rabbit rabbit)
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

    public Task<Pair?> GetMostRecentPairByRabbitIdsAsync(int rabbitId1, int rabbitId2)
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
