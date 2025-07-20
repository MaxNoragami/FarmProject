using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.OrderRequestService.Validators;

public class UpdateOrderRequestStatusParamValidator : AbstractValidator<UpdateOrderRequestStatusParam>
{
    public UpdateOrderRequestStatusParamValidator()
    {
        RuleFor(x => x.OrderRequestId)
            .GreaterThan(0)
            .WithMessage("Order Request Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Selected Order Request Status does not exist")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
