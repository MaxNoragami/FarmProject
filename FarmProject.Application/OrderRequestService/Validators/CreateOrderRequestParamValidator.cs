using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.OrderRequestService.Validators;

public class CreateOrderRequestParamValidator : AbstractValidator<CreateOrderRequestParam>
{
    public CreateOrderRequestParamValidator()
    {
        RuleFor(x => x.OrderId)
            .GreaterThan(0)
            .WithMessage("Order Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.CageId)
            .GreaterThan(0)
            .WithMessage("Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
