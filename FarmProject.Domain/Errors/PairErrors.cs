using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class PairErrors
{
    public static readonly Error NotFound = new(
        "Pair.NotFound", "Pair not found");

    public static readonly Error InvalidPairing = new(
        "Pair.InvalidPairing", "Those animals are not able to be paired");
}
