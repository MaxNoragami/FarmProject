using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class SacrificationErrors
{
    public static readonly Error NotFound = new(
        "Sacrification.NotFound", "Sacrification not found");
}
