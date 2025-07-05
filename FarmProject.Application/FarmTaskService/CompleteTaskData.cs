namespace FarmProject.Application.FarmTaskService;

public record CompleteTaskData(
    int? NewCageId,

    int? OtherCageId,
    int? FemaleOffspringCount
);
