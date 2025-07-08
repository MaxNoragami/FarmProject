using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.BirthService.Validators;

public class RecordBirthParamValidator : AbstractValidator<RecordBirthParam>
{
    public RecordBirthParamValidator()
    {
        RuleFor(x => x.breedingRabbitId)
            .GreaterThan(0)
            .WithMessage("Breeding Rabbit Id must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.offspringCount)
            .GreaterThan(-1)
            .WithMessage("Offspring Count must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
