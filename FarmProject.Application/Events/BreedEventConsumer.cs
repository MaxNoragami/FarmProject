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
        var rabbit1 = await _breedingRabbitRepository
            .GetByIdAsync(domainEvent.BreedingRabbitIds[0]);
        if (rabbit1 == null)
            return Result.Failure(ConsumerErrors.BreedingRabbitNotFound);

        var rabbit2 = await _breedingRabbitRepository
            .GetByIdAsync(domainEvent.BreedingRabbitIds[1]);
        if (rabbit2 == null)
            return Result.Failure(ConsumerErrors.BreedingRabbitNotFound);

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
