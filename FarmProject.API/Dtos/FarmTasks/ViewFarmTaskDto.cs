using FarmProject.Domain.Constants;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;

namespace FarmProject.API.Dtos.FarmTasks;

public class ViewFarmTaskDto
{
    public int Id { get; set; }
    public FarmTaskType FarmTaskType { get; set; }
    public string Message { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime DueOn { get; set; }
}
