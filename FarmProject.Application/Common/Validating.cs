using FarmProject.Domain.Common;
using FarmProject.Domain.Errors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace FarmProject.Application.Common;

public class ValidationHelper(IServiceProvider serviceProvider)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResult> ValidateAndExecute<TParam, TResult>(
    TParam parameter,
    Func<Task<TResult>> operation)
    where TResult : Result
    {
        var validator = _serviceProvider.GetService<IValidator<TParam>>();

        if (validator != null)
        {
            var validResult = await validator.ValidateAsync(parameter);

            if (!validResult.IsValid)
            {
                var firstError = validResult.Errors[0];
                var errorCode = !string.IsNullOrEmpty(firstError.ErrorCode)
                    ? firstError.ErrorCode
                    : ValidationErrors.Codes.ValidationFailed;

                var error = new Error(errorCode, firstError.ErrorMessage);

                return CreateFailureResult<TResult>(error);
            }
        }

        return await operation();
    }

    private static TResult CreateFailureResult<TResult>(Error error) where TResult : Result
    {
        var resultType = typeof(TResult);

        if (resultType.IsGenericType)
        {
            var genericResultType = resultType.GetGenericTypeDefinition();
            if (genericResultType == typeof(Result<>))
            {
                var innerType = resultType.GenericTypeArguments[0];

                var failureMethod = typeof(Result)
                    .GetMethods()
                    .FirstOrDefault(m => m.Name == nameof(Result.Failure) &&
                               m.IsGenericMethod &&
                               m.GetParameters().Length == 1 &&
                               m.GetParameters()[0].ParameterType == typeof(Error));

                if (failureMethod != null)
                {
                    var typedFailureMethod = failureMethod.MakeGenericMethod(innerType);
                    return (TResult)typedFailureMethod.Invoke(null, [error])!;
                }
            }
        }

        return (TResult)Result.Failure(error);
    }
}
