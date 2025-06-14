using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.BreedingRabbitsService.Validators;

public class AddBreedingRabbitParamValidator : AbstractValidator<AddBreedingRabbitParam>
{
    private readonly IBreedingRabbitRepository _repository;

    public AddBreedingRabbitParamValidator(IBreedingRabbitRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Breeding rabbit name cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MinimumLength(3)
            .WithMessage("Breeding rabbit name must not be shorter than 3 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(256)
            .WithMessage("Breeding rabbit name must not be longer than 265 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MustAsync(BeUniqueName)
            .WithMessage("Breeding rabbit name must be unique")
            .WithErrorCode(ValidationErrors.Codes.DuplicateName);

        RuleFor(x => x.CageId)
            .GreaterThan(0)
            .WithMessage("Cage ID must be a positive number")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        => !await _repository.IsNameUsedAsync(name, cancellationToken);
}
