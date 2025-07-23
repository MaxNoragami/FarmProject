using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.OrderService.Validators;

public class CreateOrderParamValidator : AbstractValidator<CreateOrderParam>
{
    public CreateOrderParamValidator()
    {
        RuleFor(x => x.CustomerId)
            .GreaterThan(0)
            .WithMessage("Customer Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.OrderRequests)
            .NotNull()
            .WithMessage("Order requests cannot be null")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .NotEmpty()
            .WithMessage("Order must contain at least one order request")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
