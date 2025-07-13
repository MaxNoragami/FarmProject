using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public static class CageErrors
{
    public static readonly Error NoBreedingRabbit = new(
        "Cage.NoBreedingRabbit", "No breeding rabbit is assigned to this cage");

    public static readonly Error InvalidAssignment = new(
        "Cage.InvalidAssignment", "No null reference of a breeding rabbit can be assigned to a cage");

    public static readonly Error NegativeOffspringNum = new(
        "Cage.NegativeOffspringNum", "Cannot add or remove negative num of offsprings");

    public static readonly Error OverOffspringNum = new(
        "Cage.OverOffspringNum", "Cannot remove or sacrifice more offsprings than available");

    public static readonly Error NotFound = new(
        "Cage.NotFound", "Cage not found");

    public static readonly Error RabbitAlreadyInCage = new(
        "Cage.RabbitAlreadyInCage", "The breeding rabbit is already in a cage");

    public static readonly Error RabbitNotInCage = new(
        "Cage.RabbitNotInCage", "The breeding rabbit is not currently in any cage");

    public static readonly Error Occupied = new(
        "Cage.Occupied", "This cage already has a breeding rabbit assigned");

    public static readonly Error MovementFailure = new(
        "Cage.MovementFailure", "An unexpected error occurred while moving the breeding rabbit to another cage");

    public static readonly Error InvalidSeparation = new(
        "Cage.OffspringSeparationFailure", "Separation of offsprings is not possible with the set parameters");

    public static readonly Error InvalidSacrificeCount = new(
        "Cage.InvalidSacrificeCount", "Cannot sacrifice zero or negative num of offsprings");

    public static readonly Error NotSacrificable = new(
        "Cage.NotSacrificable", "Offsprings are not ready to be sacrificed");
}
