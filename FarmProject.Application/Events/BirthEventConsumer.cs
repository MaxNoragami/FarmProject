using FarmProject.Application.FarmTaskService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;
using FarmProject.Domain.Events;
using FarmProject.Domain.Models;

namespace FarmProject.Application.Events;

public class BirthEventConsumer(
        IFarmTaskRepository farmTaskRepository)
    : IEventConsumer<BirthEvent>
{
    private readonly IFarmTaskRepository _farmTaskRepository = farmTaskRepository;

    public async Task<Result> ConsumeAsync(BirthEvent domainEvent)
    {
        try
        {
            var birthDate = domainEvent.BirthDate;

            var kitsWeaningDate = birthDate.AddDays(30);
            var removeNestDate = birthDate.AddDays(30);
            var separateOffspringsDate = birthDate.AddDays(56);

            var kitsWeaningTask = new FarmTask(
                farmTaskType: FarmTaskType.Weaning,
                message: $"The {domainEvent.OffspringCount} offsprings must be moved out from cage #{domainEvent.CageId}",
                createdOn: birthDate,
                dueOn: kitsWeaningDate
            );

            var removeNestTask = new FarmTask(
                farmTaskType: FarmTaskType.NestRemoval,
                message: $"Remove nest from cage #{domainEvent.CageId}",
                createdOn: birthDate,
                dueOn: removeNestDate
            );

            var separateOffspringsTask = new FarmTask(
                farmTaskType: FarmTaskType.OffspringSeparation,
                $"The {domainEvent.OffspringCount} offsprings from cage #{domainEvent.CageId} have to be separated by gender",
                createdOn: birthDate,
                dueOn: separateOffspringsDate
            );

            await _farmTaskRepository.AddAsync(kitsWeaningTask);
            await _farmTaskRepository.AddAsync(removeNestTask);
            await _farmTaskRepository.AddAsync(separateOffspringsTask);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(ConsumerErrors.ProcessingFailed);
        }
    }
}
