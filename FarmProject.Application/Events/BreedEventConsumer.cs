using FarmProject.Application.PairingService;
using FarmProject.Application.BreedingRabbitsService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class BreedEventConsumer(IPairingRepository pairingRepository, 
        IBreedingRabbitRepository breedingRabbitRepository) 
    : IEventConsumer<BreedEvent>
{
    private readonly IPairingRepository _pairingRepository = pairingRepository;
    private readonly IBreedingRabbitRepository _breedingRabbitRepository = breedingRabbitRepository;

    public async Task<Result> ConsumeAsync(BreedEvent domainEvent)
    {
        var breedingRabbit = await _breedingRabbitRepository
            .GetByIdAsync(domainEvent.BreedingRabbitIds[0]);
        if (breedingRabbit == null)
            return Result.Failure(ConsumerErrors.BreedingRabbitNotFound);

        var maleRabbitId = domainEvent.BreedingRabbitIds[1];
        
        try
        {
            var pair = new Pair(maleRabbitId, breedingRabbit, domainEvent.StartDate);
            await _pairingRepository.AddAsync(pair);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
