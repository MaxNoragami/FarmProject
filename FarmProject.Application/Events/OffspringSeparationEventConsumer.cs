using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class OffspringSeparationEventConsumer(
        IUnitOfWork unitOfWork)
    : IEventConsumer<OffspringSeparationEvent>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> ConsumeAsync(OffspringSeparationEvent domainEvent)
    {
        try
        {
            var separationDate = domainEvent.CreatedOn.AddDays(DomainRules.OffspringSeparationInDays);

            var separateOffspringsTask = new FarmTask(
                farmTaskType: FarmTaskType.OffspringSeparation,
                message: $"The offsprings in cage #{domainEvent.NewCageId} have to be separated by gender",
                createdOn: domainEvent.CreatedOn,
                dueOn: separationDate,
                cageId: domainEvent.NewCageId
            );

            await _unitOfWork.FarmTaskRepository.AddAsync(separateOffspringsTask);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
