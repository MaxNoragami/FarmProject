using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;

public class FarmEvent(int id, 
                       FarmEventType farmEventType, 
                       string message, 
                       DateTime createdOn, 
                       DateTime dueOn) 
             : Entity(id)
{
    public FarmEventType FarmEventType { get; private set; } = farmEventType;
    public string Message { get; private set; } = message;
    public bool IsCompleted { get; private set; }
    public DateTime CreatedOn { get; private set; } = createdOn;
    public DateTime DueOn { get; private set; } = dueOn;

    public Result MarkAsCompleted()
    {
        IsCompleted = true;
        return Result.Success();
    }

}
