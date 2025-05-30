
using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Models;
using FluentAssertions;

namespace FarmProject.Domain.UnitTests.ListnerTest;

public class Test
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

        var eventConsumer = new EventConsumer(rabbitRepository)
        {
            PairRepo = pairingRepository
        };
        var startDate = DateTime.Now;
        var breedEvent = new BreedEvent()
        {
            RabbitIds = [1, 2],
            StartDate = startDate
        };

        await eventConsumer.Consume(breedEvent);

        var pairingResult = await pairingRepository.GetAllAsync();

        Assert.Single(pairingResult);

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

    public Task RemoveAsync(Pair pair)
    {
        throw new NotImplementedException();
    }

    public Task<Pair> UpdateAsync(Pair pair)
    {
        throw new NotImplementedException();
    }
}

internal class EventConsumer
{
    

    public EventConsumer(IRabbitRepository rabbitRepository)
    {
        _rabbitRepository = rabbitRepository;
    }

    public IPairingRepository PairRepo { get; internal set; }
    private readonly IRabbitRepository _rabbitRepository;

    internal async Task Consume(BreedEvent breedEvent)
    {
        var femaleRabbit = await _rabbitRepository.GetByIdAsync(breedEvent.RabbitIds[0]);
        var maleRabbit = await _rabbitRepository.GetByIdAsync(breedEvent.RabbitIds[1]);

        Rabbit temp;
        if(femaleRabbit.Gender == Gender.Male)
        {
            temp = femaleRabbit;
            femaleRabbit = maleRabbit;
            maleRabbit = temp;
        }

        await PairRepo.AddAsync(new Pair(maleRabbit, 
                femaleRabbit, 
                breedEvent.StartDate));
    }
}