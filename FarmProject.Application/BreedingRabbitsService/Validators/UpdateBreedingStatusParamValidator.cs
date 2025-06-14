using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.BreedingRabbitsService.Validators;

public class UpdateBreedingStatusParamValidator : AbstractValidator<UpdateBreedingStatusParam>
{
    public UpdateBreedingStatusParamValidator()
    {
        RuleFor(x => x.BreedingRabbitId)
            .GreaterThan(0)
            .WithMessage("Breeding Rabbit Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.BreedingStatus)
            .IsInEnum()
            .WithMessage("Selected Breeding Status does not exist")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
