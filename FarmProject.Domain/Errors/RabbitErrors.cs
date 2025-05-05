using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class RabbitErrors
{
    public static readonly Error NotFound = new(
        "Rabbit.NotFound", "Rabbit not found");

    public static readonly Error InvalidBreedingStatus = new(
        "Rabbit.InvalidBreedingStatus", "Breeding status cannot be applied to this gender");
}
