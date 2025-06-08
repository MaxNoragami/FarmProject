using FarmProject.Domain.Common;
using Microsoft.Extensions.Logging;

namespace FarmProject.Application.Common;

public sealed class LoggingHelper(ILogger<LoggingHelper> logger)
{
    private readonly ILogger<LoggingHelper> _logger = logger;

    public async Task<TResult> LogOperation<TResult>(
            string operationName,
            Func<Task<TResult>> operation)
        where TResult : Result
    {
        _logger.LogInformation(
            "Starting operation {OperationName}, {DateTimeUtc}",
            operationName,
            DateTime.UtcNow);

        TResult result;

        try
        {
            result = await operation();

            if (result.IsFailure)
                _logger.LogError(
                    "Operation failed {OperationName}, Error: {Error}, {DateTimeUtc}",
                    operationName,
                    result.Error,
                    DateTime.UtcNow);
            else
                _logger.LogInformation(
                    "Completed operation {OperationName}, {DateTimeUtc}",
                    operationName,
                    DateTime.UtcNow);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Exception in {OperationName}, {DateTimeUtc}",
                operationName,
                DateTime.UtcNow);

            if (typeof(TResult).IsGenericType)
            {
                var valueType = typeof(TResult).GenericTypeArguments[0];

                var genericFailure = typeof(Result).GetMethod("Failure", new[] { typeof(Error) })
                    .MakeGenericMethod(valueType)
                    .Invoke(null, new object[] { new Error(
                "UnhandledException",
                "An unexpected error occurred while processing the request"
            )});

                return (TResult)genericFailure;
            }
            else
            {
                return (TResult)Result.Failure(new Error(
                    "UnhandledException",
                    "An unexpected error occurred while processing the request"
                ));
            }
        }
    }
}
