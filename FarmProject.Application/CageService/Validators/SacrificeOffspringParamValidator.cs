using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.CageService.Validators;

public class SacrificeOffspringParamValidator : AbstractValidator<SacrificeOffspringParam>
{
    public SacrificeOffspringParamValidator()
    {
        RuleFor(x => x.CageId)
            .GreaterThan(0)
            .WithMessage("Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Count)
            .GreaterThan(0)
            .WithMessage("Count must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
