using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class OrderErrors
{
    public static readonly Error NotFound = new(
        "Order.NotFound", "Order not found");
}
