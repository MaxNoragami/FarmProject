using FarmProject.Application.Common.Models;
using FarmProject.Domain.Errors;
using FluentValidation;

namespace FarmProject.Application.Common.Validators;

public record PaginatedRequestParam<T>(PaginatedRequest<T> Request);

public class PaginatedRequestParamValidator<T> : AbstractValidator<PaginatedRequestParam<T>> where T : class
{
    public PaginatedRequestParamValidator()
    {
        RuleFor(x => x.Request.PageSize)
            .InclusiveBetween(1, 128)
            .WithMessage("Page size must be between 1 and 128")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);

        RuleFor(x => x.Request.PageIndex)
            .GreaterThan(0)
            .WithMessage("Page index must be greater than 0")
            .WithErrorCode(ValidationErrors.Codes.InvalidInput);
    }
}
