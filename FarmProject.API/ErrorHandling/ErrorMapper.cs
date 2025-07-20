using FarmProject.Application.IdentityService;
using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.ErrorHandling;

public static class ErrorMapper
{
    public static ActionResult<T> ToActionResult<T>(this Error error)
    {
        var (statusCode, apiError) = MapToApiError(error);

        return statusCode switch
        {
            StatusCodes.Status404NotFound => new NotFoundObjectResult(apiError),
            StatusCodes.Status400BadRequest => new BadRequestObjectResult(apiError),
            _ => new ObjectResult(apiError) { StatusCode = statusCode }
        };
    }

    private static (int statusCode, ApiError apiError) MapToApiError(Error error)
    {
        int statusCode;

        if (error.Code.EndsWith(".NotFound"))
            statusCode = StatusCodes.Status404NotFound;
        else if (error.Code.Equals(BreedingRabbitErrors.NotAvailableToPair.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.InvalidAssignment.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.RabbitAlreadyInCage.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(BreedingRabbitErrors.NoChangesRequested.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(BreedingRabbitErrors.NotPregnant.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(BreedingRabbitErrors.InvalidBreedingStatus.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(BreedingRabbitErrors.CreationFailed.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.Occupied.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.MovementFailure.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.InvalidSeparation.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.OverOffspringNum.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.InvalidSacrificeCount.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.NotSacrificable.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.NoBreedingRabbit.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.NegativeOffspringNum.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.RabbitNotInCage.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.NoOffspring.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.InsufficientOffspring.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.InvalidReservationCount.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(CageErrors.ExceedsReservedAmount.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(FarmTaskErrors.NullValue.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(FarmTaskErrors.AlreadyCompleted.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(FarmTaskErrors.MissingParameter.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.InvalidStateChange.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.NotSuccessful.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.NoEndDate.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.InvalidOutcome.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.InvalidPairing.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.CreationFailed.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.UpdateFailed.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(OrderErrors.EmptyOrderRequests.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(SacrificationErrors.CageMismatch.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(SacrificationErrors.ExceedsOrderAmount.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(SacrificationErrors.NoBirthDate.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(SacrificationErrors.OrderRequiresOrderRequest.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(SacrificationErrors.OrderRequestNotAllowed.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(SacrificationErrors.InsufficientAvailableOffspring.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(OrderRequestErrors.InvalidAmount.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(ValidationErrors.Codes.ValidationFailed, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(ValidationErrors.Codes.DuplicateName, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(ValidationErrors.Codes.DuplicateEmail, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(ValidationErrors.Codes.DuplicatePhoneNum, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(ValidationErrors.Codes.InvalidInput, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(IdentityErrors.InvalidCredentials.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(IdentityErrors.UserAlreadyExists.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(IdentityErrors.InvalidRole.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(IdentityErrors.Codes.RegistrationFailed, StringComparison.OrdinalIgnoreCase)
        )
            statusCode = StatusCodes.Status400BadRequest;
        else
            statusCode = StatusCodes.Status500InternalServerError;

        return (statusCode, new ApiError(error.Code, error.Description));
    }
}
