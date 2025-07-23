using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class BreedEventConsumer( 
        IUnitOfWork unitOfWork) 
    : IEventConsumer<BreedEvent>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> ConsumeAsync(BreedEvent domainEvent)
    {
        var breedingRabbit = await _unitOfWork.BreedingRabbitRepository
            .GetByIdAsync(domainEvent.BreedingRabbitIds[0]);
        if (breedingRabbit == null)
            return Result.Failure(ConsumerErrors.BreedingRabbitNotFound);

        var maleRabbitId = domainEvent.BreedingRabbitIds[1];
        
        try
        {
            var pair = new Pair(maleRabbitId, breedingRabbit, domainEvent.StartDate);
            await _unitOfWork.PairingRepository.AddAsync(pair);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
