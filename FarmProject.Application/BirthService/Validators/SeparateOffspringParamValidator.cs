using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.BirthService.Validators;

public class SeparateOffspringParamValidator : AbstractValidator<SeparateOffspringParam>
{
    public SeparateOffspringParamValidator()
    {
        RuleFor(x => x.femaleOffspringCount)
            .GreaterThan(0)
            .WithMessage("Female Offspring Count must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.currentCageId)
            .GreaterThan(0)
            .WithMessage("Current Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.otherCageId)
            .GreaterThan(0)
            .WithMessage("Other Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
