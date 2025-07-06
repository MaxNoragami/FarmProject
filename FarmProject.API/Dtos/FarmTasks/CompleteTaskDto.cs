namespace FarmProject.API.Dtos.FarmTasks;

public class CompleteTaskDto
{
    public int? NewCageId { get; set; }

    public int? OtherCageId { get; set; }
    public int? FemaleOffspringCount { get; set; }
}