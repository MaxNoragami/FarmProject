using FarmProject.Domain.Models;

namespace FarmProject.API.Dtos.FarmTasks;

public static class FarmTaskMapper
{
    public static ViewFarmTaskDto ToViewFarmTaskDto(this FarmTask farmTask)
        => new ViewFarmTaskDto()
        {
            Id = farmTask.Id,
            FarmTaskType = farmTask.FarmTaskType,
            Message = farmTask.Message,
            IsCompleted = farmTask.IsCompleted,
            CreatedOn = farmTask.CreatedOn,
            DueOn = farmTask.DueOn
        };
}
