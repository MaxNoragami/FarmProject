using FarmProject.Domain.Constants;

namespace FarmProject.Presentation.Models.FarmTasks;

public class ViewFarmTaskDto
{
    public int Id { get; set; }
    public FarmTaskType FarmTaskType { get; set; }
    public string Message { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime DueOn { get; set; }
}
