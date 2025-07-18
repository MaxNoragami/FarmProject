using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class OrderRequestErrors
{
    public static readonly Error NotFound = new(
        "OrderRequest.NotFound", "Order request not found");

    public static readonly Error InvalidAmount = new(
        "OrderRequest.InvalidAmount", "Sacrifice amount is invalid compared to the reserved amount");
}
