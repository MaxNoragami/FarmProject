namespace FarmProject.API.Middlewares;

public class TimingMiddleware(ILogger<TimingMiddleware> logger, RequestDelegate next)
{
    private readonly ILogger<TimingMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        var start = DateTime.UtcNow;
        await _next.Invoke(context);
        _logger
            .LogInformation($"Timing {context.Request.Path}: {(DateTime.UtcNow - start).TotalMilliseconds}ms");
    }
}

public static class TimingExtensions
{
    public static IApplicationBuilder UseTiming(this IApplicationBuilder app)
        => app.UseMiddleware<TimingMiddleware>();
}
