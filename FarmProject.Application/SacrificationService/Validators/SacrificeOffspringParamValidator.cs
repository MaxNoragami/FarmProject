using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.SacrificationService.Validators;

public class SacrificeOffspringParamValidator : AbstractValidator<SacrificeOffspringParam>
{
    public SacrificeOffspringParamValidator()
    {
        RuleFor(x => x.CageId)
            .GreaterThan(0)
            .WithMessage("Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.SacrificationReason)
            .IsInEnum()
            .WithMessage("Selected Sacrification Reason does not exist")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        When(x => x.OrderRequestId.HasValue, () => {
            RuleFor(x => x.OrderRequestId!.Value)
                .GreaterThan(0)
                .WithMessage("Order Request Id must be a positive number")
                .WithErrorCode(ValidationErrors.Codes.InvalidInput);
        });
    }
}
