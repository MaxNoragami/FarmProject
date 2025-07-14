using FarmProject.Domain.Common;

namespace FarmProject.Domain.Errors;

public class CustomerErrors
{
    public static readonly Error NotFound = new(
        "Customer.NotFound", "Customer not found");
}
