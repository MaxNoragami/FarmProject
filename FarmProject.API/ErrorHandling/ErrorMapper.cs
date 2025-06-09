using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FarmProject.API.ErrorHandling;

public static class ErrorMapper
{
    public static ActionResult<T> ToActionResult<T>(this Error error)
    {
        var (statusCode, apiError) = MapToApiError(error);
        return new ObjectResult(apiError) { StatusCode = statusCode };
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
            || error.Code.Equals(CageErrors.Occupied.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(FarmTaskErrors.NullValue.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(FarmTaskErrors.AlreadyCompleted.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.InvalidStateChange.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.NotSuccessful.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.NoEndDate.Code, StringComparison.OrdinalIgnoreCase)
            || error.Code.Equals(PairErrors.InvalidOutcome.Code, StringComparison.OrdinalIgnoreCase)
        )
            statusCode = StatusCodes.Status400BadRequest;
        else
            statusCode = StatusCodes.Status500InternalServerError;

        return (statusCode, new ApiError(error.Code, error.Description));
    }
}
