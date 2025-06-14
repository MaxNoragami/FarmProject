using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.PairingService.Validators;

public class UpdatePairingStatusParamValidator : AbstractValidator<UpdatePairingStatusParam>
{
    public UpdatePairingStatusParamValidator()
    {
        RuleFor(x => x.PairId)
            .GreaterThan(0)
            .WithMessage("Pair Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.PairingStatus)
            .IsInEnum()
            .WithMessage("Selected Pairing Status does not exist")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
