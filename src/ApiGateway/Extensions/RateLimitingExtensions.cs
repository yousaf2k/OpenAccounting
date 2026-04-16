using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace ApiGateway.Extensions;

/// <summary>
/// Extension methods for configuring rate limiting.
/// </summary>
public static class RateLimitingExtensions
{
    /// <summary>
    /// Adds rate limiting configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection with rate limiting configured.</returns>
    public static IServiceCollection AddRateLimitingConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            // Set global rate limiter policy as the default
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                var globalConfig = configuration.GetSection("RateLimiting:Global");
                var window = TimeSpan.Parse(globalConfig["Window"] ?? "00:01:00");
                var permitLimit = int.Parse(globalConfig["PermitLimit"] ?? "100");

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        Window = window,
                        PermitLimit = permitLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.AddPolicy("Global", httpContext =>
            {
                var globalConfig = configuration.GetSection("RateLimiting:Global");
                var window = TimeSpan.Parse(globalConfig["Window"] ?? "00:01:00");
                var permitLimit = int.Parse(globalConfig["PermitLimit"] ?? "100");

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        Window = window,
                        PermitLimit = permitLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.AddPolicy("Authentication", httpContext =>
            {
                var authConfig = configuration.GetSection("RateLimiting:Authentication");
                var window = TimeSpan.Parse(authConfig["Window"] ?? "00:01:00");
                var permitLimit = int.Parse(authConfig["PermitLimit"] ?? "20");

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        Window = window,
                        PermitLimit = permitLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.AddPolicy("Api", httpContext =>
            {
                var apiConfig = configuration.GetSection("RateLimiting:Api");
                var window = TimeSpan.Parse(apiConfig["Window"] ?? "01:00:00");
                var permitLimit = int.Parse(apiConfig["PermitLimit"] ?? "1000");

                // For authenticated users, partition by user ID
                var userId = httpContext.User?.Identity?.Name ?? "anonymous";

                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: userId,
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        Window = window,
                        SegmentsPerWindow = 4,
                        PermitLimit = permitLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.AddPolicy("Reporting", httpContext =>
            {
                var reportingConfig = configuration.GetSection("RateLimiting:Reporting");
                var permitLimit = int.Parse(reportingConfig["PermitLimit"] ?? "5");

                // For authenticated users, partition by user ID
                var userId = httpContext.User?.Identity?.Name ?? "anonymous";

                return RateLimitPartition.GetConcurrencyLimiter(
                    partitionKey: userId,
                    factory: _ => new ConcurrencyLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                context.HttpContext.Response.ContentType = "application/json";

                var retryAfter = context.Lease.GetAllMetadata()
                    .FirstOrDefault(m => m.Key == MetadataName.RetryAfter.Name);

                var response = new
                {
                    error = new
                    {
                        code = "TooManyRequests",
                        message = "Rate limit exceeded. Please try again later.",
                        retryAfter = retryAfter.Value
                    }
                };

                await context.HttpContext.Response.WriteAsJsonAsync(response, token);
            };
        });

        return services;
    }
}