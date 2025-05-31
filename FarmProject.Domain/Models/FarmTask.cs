using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;

namespace FarmProject.Domain.Models;

public class FarmTask(FarmTaskType farmTaskType, 
                       string message, 
                       DateTime createdOn, 
                       DateTime dueOn) 
             : Entity
{
    public FarmTaskType FarmTaskType { get; private set; } = farmTaskType;
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
