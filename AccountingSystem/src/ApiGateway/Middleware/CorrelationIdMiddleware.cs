namespace ApiGateway.Middleware;

/// <summary>
/// Middleware for generating and propagating correlation IDs.
/// </summary>
public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CorrelationIdMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger.</param>
    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = GetOrGenerateCorrelationId(context);

        // Add correlation ID to response headers
        context.Response.Headers["X-Correlation-Id"] = correlationId;

        // Add correlation ID to request headers for downstream services
        context.Request.Headers["X-Correlation-Id"] = correlationId;

        // Add correlation ID to logging context
        using (_logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationId
        }))
        {
            await _next(context);
        }
    }

    private static string GetOrGenerateCorrelationId(HttpContext context)
    {
        // Try to get correlation ID from request headers
        if (context.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId) &&
            !string.IsNullOrEmpty(correlationId))
        {
            return correlationId.ToString();
        }

        if (context.Request.Headers.TryGetValue("x-correlation-id", out correlationId) &&
            !string.IsNullOrEmpty(correlationId))
        {
            return correlationId.ToString();
        }

        // Try to get from activity (OpenTelemetry)
        var activityCorrelationId = System.Diagnostics.Activity.Current?.Id;
        if (!string.IsNullOrEmpty(activityCorrelationId))
        {
            return activityCorrelationId;
        }

        // Generate new correlation ID
        return Guid.NewGuid().ToString();
    }
}