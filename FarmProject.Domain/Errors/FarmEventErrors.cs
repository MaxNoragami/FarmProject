using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class FarmEventErrors
{
    public static readonly Error NotFound = new(
        "FarmEvent.NotFound", "FarmEvent not found");

    public static readonly Error NullValue = new(
        "FarmEvent.NullValue", "FarmEvent cannot be null");
}
