using FarmProject.Domain.Common;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using FarmProject.Domain.Errors;

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

                var resultType = typeof(TResult);
                var genericType = resultType.GetGenericArguments().FirstOrDefault();

                if (genericType != null)
                {
                    var failureMethod = typeof(Result)
                        .GetMethods()
                        .FirstOrDefault(m => m.Name == "Failure" &&
                               m.IsGenericMethod &&
                               m.GetParameters().Length == 1 &&
                               m.GetParameters()[0].ParameterType == typeof(Error));

                    if (failureMethod != null)
                    {
                        return (TResult)failureMethod
                            .MakeGenericMethod(genericType)
                            .Invoke(null, [error])!;
                    }
                }

                return (TResult)Result.Failure(error);
            }
        }

        return await operation();
    }
}
