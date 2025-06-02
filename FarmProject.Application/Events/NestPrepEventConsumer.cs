using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class NestPrepEventConsumer(IFarmTaskRepository farmTaskRepository)
    : IEventConsumer<NestPrepEvent>
{
    public readonly IFarmTaskRepository _farmTaskRepository = farmTaskRepository;

    public async Task<Result> ConsumeAsync(NestPrepEvent domainEvent)
    {
        try
        {
            var farmTask = new FarmTask(
                farmTaskType: FarmTaskType.NestPreparation,
                message: domainEvent.Message,
                createdOn: domainEvent.CreatedOn,
                dueOn: domainEvent.DueDate
            );

            await _farmTaskRepository.AddAsync(farmTask);
            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
