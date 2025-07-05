using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class OffspringSeparationEventConsumer(
        IFarmTaskRepository farmTaskRepository)
    : IEventConsumer<OffspringSeparationEvent>
{
    private readonly IFarmTaskRepository _farmTaskRepository = farmTaskRepository;

    public async Task<Result> ConsumeAsync(OffspringSeparationEvent domainEvent)
    {
        try
        {
            var separationDate = domainEvent.CreatedOn.AddDays(26);

            var separateOffspringsTask = new FarmTask(
                farmTaskType: FarmTaskType.OffspringSeparation,
                message: $"The offsprings in cage #{domainEvent.NewCageId} have to be separated by gender",
                createdOn: domainEvent.CreatedOn,
                dueOn: separationDate,
                cageId: domainEvent.NewCageId
            );

            await _farmTaskRepository.AddAsync(separateOffspringsTask);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
