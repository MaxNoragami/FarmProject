using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class FarmTaskErrors
{
    public static readonly Error NotFound = new(
        "FarmTask.NotFound", "Farm task not found");

    public static readonly Error NullValue = new(
        "FarmTask.NullValue", "Farm task cannot be null");

    public static readonly Error AlreadyCompleted = new(
        "FarmTask.AlreadyCompleted", "Farm task is already marked as completed");

    public static readonly Error MissingParameter = new(
        "FarmTask.MissingParameter", "One or more parameters are missing");
}
