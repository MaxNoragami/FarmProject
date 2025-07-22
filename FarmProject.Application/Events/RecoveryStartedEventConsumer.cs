using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class RecoveryStartedEventConsumer(
        IUnitOfWork unitOfWork) 
    : IEventConsumer<RecoveryStartedEvent>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> ConsumeAsync(RecoveryStartedEvent domainEvent)
    {
        try
        {
            var availableDate = domainEvent.RecoveryStartDate.AddDays(DomainRules.RecoveryPeriodInDays);

            var recoveryTask = new FarmTask(
                farmTaskType: FarmTaskType.BreedingStatusUpdate,
                message: $"Breeding rabbit #{domainEvent.BreedingRabbitId} recovery period is complete",
                createdOn: domainEvent.RecoveryStartDate,
                dueOn: availableDate,
                breedingRabbitId: domainEvent.BreedingRabbitId
            );

            await _unitOfWork.FarmTaskRepository.AddAsync(recoveryTask);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
