using FarmProject.Application;

namespace FarmProject.API.Middlewares;

public class TransactionMiddleware(
    RequestDelegate next, 
    ILogger<TransactionMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<TransactionMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, IUnitOfWork unitOfWork)
    {
        if (IsWriteOperation(context.Request.Method))
        {
            _logger.LogInformation("Start transaction for {Method} {Path}",
                context.Request.Method, context.Request.Path);

            await unitOfWork.BeginTransactionAsync();

            try
            {
                await _next(context);

                if (IsSuccessStatusCode(context.Response.StatusCode))
                {
                    await unitOfWork.CommitTransactionAsync();
                    _logger.LogInformation("Transaction committed for {Method} {Path}",
                        context.Request.Method, context.Request.Path);
                }
                else
                {
                    await unitOfWork.RollbackTransactionAsync();
                    _logger.LogInformation(
                        "Transaction rolled back for {Method} {Path} with status code {Code}",
                        context.Request.Method, context.Request.Path, context.Response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                await unitOfWork.RollbackTransactionAsync();
                _logger.LogError(ex, "Exception while processing {Method} {Path}",
                    context.Request.Method, context.Request.Path);
                throw;
            }
        }
        else
            await _next(context);
    }

    private static bool IsSuccessStatusCode(int statusCode)
        => statusCode >= 200 && statusCode < 300;

    private static bool IsWriteOperation(string method)
        => method is "POST" or "PUT" or "PATCH" or "DELETE";
}

public static class TransactionExtensions
{
    public static IApplicationBuilder UseTransactions(this IApplicationBuilder app)
        => app.UseMiddleware<TransactionMiddleware>();
}