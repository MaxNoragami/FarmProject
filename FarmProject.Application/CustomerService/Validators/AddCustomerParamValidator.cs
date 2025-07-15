using FarmProject.Application.CageService;
using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.CustomerService.Validators;

public class AddCustomerParamValidator : AbstractValidator<AddCustomerParam>
{
    private readonly ICustomerRepository _repository;

    public AddCustomerParamValidator(ICustomerRepository repository)
    {
        _repository = repository;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MinimumLength(3)
            .WithMessage("First name must not be shorter than 3 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(64)
            .WithMessage("First name must not be longer than 64 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MinimumLength(3)
            .WithMessage("Last name must not be shorter than 3 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(64)
            .WithMessage("Last name must not be longer than 64 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MinimumLength(3)
            .WithMessage("Email must not be shorter than 3 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(64)
            .WithMessage("Email must not be longer than 64 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .EmailAddress()
            .WithMessage("Email format is invalid")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MustAsync(BeUniqueEmail)
            .WithMessage("Customer email must be unique")
            .WithErrorCode(ValidationErrors.Codes.DuplicateEmail);

        RuleFor(x => x.PhoneNum)
            .NotEmpty()
            .WithMessage("Phone number cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MinimumLength(3)
            .WithMessage("Phone number must not be shorter than 3 digits")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(16)
            .WithMessage("Phone number must not be longer than 16 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .Matches(@"^\+?[\d\s\-\(\)]*$")
            .WithMessage("Phone number format is invalid")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MustAsync(BeUniquePhoneNum)
            .WithMessage("Customer phone number must be unique")
            .WithErrorCode(ValidationErrors.Codes.DuplicatePhoneNum);
    }

    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
        => !await _repository.IsEmailUsedAsync(email, cancellationToken);

    private async Task<bool> BeUniquePhoneNum(string phoneNum, CancellationToken cancellationToken)
        => !await _repository.IsPhoneNumUsedAsync(phoneNum, cancellationToken);
}
