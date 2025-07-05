using FarmProject.Application.FarmTaskService;

namespace FarmProject.API.Dtos.FarmTasks;

public static class CompleteTaskMapper
{
    public static CompleteTaskData ToCompleteTaskData(this CompleteTaskDto completeTaskDto)
        => new CompleteTaskData(
                NewCageId: completeTaskDto.NewCageId,
                
                OtherCageId: completeTaskDto.OtherCageId,
                FemaleOffspringCount: completeTaskDto.FemaleOffspringCount
            );
}
