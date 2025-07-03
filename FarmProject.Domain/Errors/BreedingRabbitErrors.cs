using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class BreedingRabbitErrors
{
    public static readonly Error NotFound = new(
        "BreedingRabbit.NotFound", "Rabbit not found");

    public static readonly Error InvalidBreedingStatus = new(
        "BreedingRabbit.InvalidBreedingStatus", "Breeding status cannot be applied to this breeding rabbot");

    public static readonly Error CreationFailed = new(
        "BreedingRabbit.CreationFailed", "An unexpected error occurred while adding the breeding rabbit");

    public static readonly Error NoChangesRequested = new(
        "BreedingRabbit.NoChangesRequested", "No changes were specified in the request");

    public static readonly Error NotAvailableToPair = new(
        "BreedingRabbit.NotAvailableToPair", "The breeding rabbit is not available to pair");

    public static readonly Error NotPregnant = new(
        "BreedingRabbit.NotPregnant", "Only preganant breeding rabbits can give birth");
}
