using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class SacrificationErrors
{
    public static readonly Error NotFound = new(
        "Sacrification.NotFound", "Sacrification not found");

    public static readonly Error CageMismatch = new(
        "Sacrification.CageMismatch", "Order request cage does not match provided cage");

    public static readonly Error ExceedsOrderAmount = new(
        "Sacrification.ExceedsOrderAmount", "Sacrification amount exceeds order request amount");

    public static readonly Error NoBirthDate = new(
        "Sacrification.NoBirthDate", "Cannot sacrifice offspring without birth date");

    public static readonly Error OrderRequiresOrderRequest = new(
        "Sacrification.OrderRequiresOrderRequest", "Order sacrifications require an order request");

    public static readonly Error OrderRequestNotAllowed = new(
        "Sacrification.OrderRequestNotAllowed", "Order request provided for non-order sacrification");

    public static readonly Error InsufficientAvailableOffspring = new(
        "Sacrification.InsufficientAvailableOffsprings", 
            "Insufficient available offsprings for sacrification, as some of them are reserved");
}
