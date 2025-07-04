using FarmProject.Application.FarmTaskService;

namespace FarmProject.API.Dtos.FarmTasks;

public static class CompleteTaskMapper
{
    public static CompleteTaskData ToCompleteTaskData(this CompleteTaskDto completeTaskDto)
        => new CompleteTaskData(
                OldCageId: completeTaskDto.OldCageId,
                NewCageId: completeTaskDto.NewCageId,
                CurrentCageId: completeTaskDto.CurrentCageId,
                OtherCageId: completeTaskDto.OtherCageId,
                FemaleOffspringCount: completeTaskDto.FemaleOffspringCount
            );
}
