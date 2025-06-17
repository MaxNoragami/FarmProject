using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.IdentityService.Validators;

public class LoginUserParamValidator : AbstractValidator<LoginUserParam>
{
    public LoginUserParamValidator()
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

            .MaximumLength(48)
            .WithMessage("Password max length is 48 characters")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
