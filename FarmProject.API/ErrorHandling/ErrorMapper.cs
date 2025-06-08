using FarmProject.Domain.Common;
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
        switch (error.Code)
        {
            case string code when code.EndsWith(".NotFound"):
                statusCode = StatusCodes.Status404NotFound;
                break;

            case "BreedingRabbit.NotAvailableToPair":
            case "Cage.InvalidAssignment":
            case "Cage.RabbitAlreadyInCage":
            case "BreedingRabbit.NoChangesRequested":
            case "Cage.Occupied":
            case "FarmTask.NullValue":
            case "FarmTask.AlreadyCompleted":
            case "Pair.InvalidStateChange":
            case "Pair.NotSuccessful":
            case "Pair.NoEndDate":
            case "Pair.InvalidOutcome":
                statusCode = StatusCodes.Status400BadRequest;
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        return (statusCode, new ApiError(error.Code, error.Description));
    }
}
