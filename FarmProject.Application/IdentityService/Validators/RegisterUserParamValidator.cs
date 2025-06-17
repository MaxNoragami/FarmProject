using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.IdentityService.Validators;

public class RegisterUserParamValidator : AbstractValidator<RegisterUserParam>
{
    public RegisterUserParamValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .WithMessage("Email field cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .EmailAddress()
            .WithMessage("Email invalid format")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(128)
            .WithMessage("Email address max length is 128 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .WithMessage("Password field cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)
            
            .MinimumLength(8)
            .WithMessage("Password must contain at least 8 chars")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)
            
            .Matches(@"[0-9]+")
            .WithMessage("Password must contain at least one digit")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .Matches(@"[a-z]+")
            .WithMessage("Password must contain at least one lowercase")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .Matches(@"[A-Z]+")
            .WithMessage("Password must contain at least one uppercase")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(48)
            .WithMessage("Password max length is 48 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Request.FirstName)
            .NotEmpty()
            .WithMessage("First name field cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(48)
            .WithMessage("First name max length is 48 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Request.LastName)
            .NotEmpty()
            .WithMessage("Last name field cannot be empty")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput)

            .MaximumLength(48)
            .WithMessage("Last name max length is 48 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Request.Role)
            .IsInEnum()
            .WithMessage("Selected role type does not exist")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
