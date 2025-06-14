using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.CageService.Validators;

public class UpdateOffspringTypeParamValidator : AbstractValidator<UpdateOffspringTypeParam>
{
    public UpdateOffspringTypeParamValidator()
    {
        RuleFor(x => x.CageId)
            .GreaterThan(0)
            .WithMessage("Cage Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.OffspringType)
            .IsInEnum()
            .WithMessage("Selected Offspring Type does not exist")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
