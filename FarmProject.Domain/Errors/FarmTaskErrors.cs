using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class FarmTaskErrors
{
    public static readonly Error NotFound = new(
        "FarmTask.NotFound", "FarmTask not found");

    public static readonly Error NullValue = new(
        "FarmTask.NullValue", "FarmTask cannot be null");
}
