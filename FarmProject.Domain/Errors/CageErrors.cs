using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class CageErrors
{
    public static readonly Error NoFemaleRabbit = new(
        "Cage.NoFemaleRabbit", "No female breeding rabbit is assigned to this cage");

    public static readonly Error NoMaleRabbit = new(
        "Cage.NoMaleRabbit", "No male breeding rabbit is assigned to this cage");

    public static readonly Error InvalidAssignment = new(
        "Cage.InvalidAssignment", "No null reference of a breeding rabbit can be assigned to a cage");

    public static readonly Error NegativeOffspringNum = new(
        "Cage.NegativeOffspringNum", "Cannot add or remove negative num of offsprings");

    public static readonly Error OverOffspringNum = new(
        "Cage.OverOffspringNum", "Cannot remove more offsprings than available");

    public static readonly Error NotFound = new(
        "Cage.NotFound", "Cage not found");

    public static readonly Error RabbitAlreadyInCage = new(
        "Cage.RabbitAlreadyInCage", "The breeding rabbit is already in a cage");
}
