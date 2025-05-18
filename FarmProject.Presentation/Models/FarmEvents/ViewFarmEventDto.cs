using FarmProject.Domain.Constants;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FarmProject.Presentation.Models.FarmEvents;

public class ViewFarmEventDto
{
    public int Id { get; set; }
    public FarmEventType FarmEventType { get; set; }
    public string Message { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime DueOn { get; set; }
}
