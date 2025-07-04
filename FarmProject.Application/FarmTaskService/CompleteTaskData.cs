namespace FarmProject.Application.FarmTaskService;

public record CompleteTaskData(
    int? OldCageId,
    int? NewCageId,

    int? CurrentCageId,
    int? OtherCageId,
    int? FemaleOffspringCount
);
