using FarmProject.Application.PairingService;
using FarmProject.Application.RabbitsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class BreedEventConsumer(IPairingRepository pairingRepository, 
        IRabbitRepository rabbitRepository) 
    : IEventConsumer<BreedEvent>
{
    private readonly IPairingRepository _pairingRepository = pairingRepository;
    private readonly IRabbitRepository _rabbitRepository = rabbitRepository;

    public async Task<Result> ConsumeAsync(BreedEvent domainEvent)
    {
        var rabbit1 = await _rabbitRepository
            .GetByIdAsync(domainEvent.RabbitIds[0]);
        if (rabbit1 == null)
            return Result.Failure(ConsumerErrors.RabbitNotFound);

        var rabbit2 = await _rabbitRepository
            .GetByIdAsync(domainEvent.RabbitIds[1]);
        if (rabbit2 == null)
            return Result.Failure(ConsumerErrors.RabbitNotFound);

        var femaleRabbit = (rabbit1.Gender == Gender.Female) ? rabbit1 : rabbit2;
        var maleRabbit = (rabbit1.Gender == Gender.Male) ? rabbit1 : rabbit2;
        
        try
        {
            var pair = new Pair(maleRabbit, femaleRabbit, domainEvent.StartDate);
            await _pairingRepository.AddAsync(pair);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
