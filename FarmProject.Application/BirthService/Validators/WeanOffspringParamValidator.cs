using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.BirthService.Validators;

public class WeanOffspringParamValidator : AbstractValidator<WeanOffspringParam>
{
    public WeanOffspringParamValidator()
    {
        RuleFor(x => x.oldCageId)
            .GreaterThan(0)
            .WithMessage("Old Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
        
        RuleFor(x => x.newCageId)
            .GreaterThan(0)
            .WithMessage("New Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
