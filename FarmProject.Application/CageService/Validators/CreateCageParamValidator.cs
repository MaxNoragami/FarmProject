using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.CageService.Validators;

public class CreateCageParamValidator : AbstractValidator<CreateCageParam>
{
    private readonly ICageRepository _repository;

    public CreateCageParamValidator(ICageRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Cage name cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MinimumLength(3)
            .WithMessage("Cage name must not be shorter than 3 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(256)
            .WithMessage("Cage name must not be longer than 265 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MustAsync(BeUniqueName)
            .WithMessage("Cage name must be unique")
            .WithErrorCode(ValidationErrors.Codes.DuplicateName);
    }

    private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        => !await _repository.IsNameUsedAsync(name, cancellationToken);
}
