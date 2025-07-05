using FarmProject.Domain.Common;
using FarmProject.Domain.Constants;
using FarmProject.Domain.Errors;

namespace FarmProject.Domain.Models;

public class FarmTask(
        FarmTaskType farmTaskType, 
        string message, 
        DateTime createdOn, 
        DateTime dueOn,
        int? cageId = null) 
    : Entity
{
    public FarmTaskType FarmTaskType { get; private set; } = farmTaskType;
    public string Message { get; private set; } = message;
    public bool IsCompleted { get; private set; }
    public DateTime CreatedOn { get; private set; } = createdOn;
    public DateTime DueOn { get; private set; } = dueOn;
    public int? CageId { get; private set; } = cageId;

    public Result MarkAsCompleted()
    {
        if (IsCompleted)
            return Result.Failure(FarmTaskErrors.AlreadyCompleted);

        IsCompleted = true;
        return Result.Success();
    }

}
