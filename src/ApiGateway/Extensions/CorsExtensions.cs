namespace ApiGateway.Extensions;

/// <summary>
/// Extension methods for configuring CORS.
/// </summary>
public static class CorsExtensions
{
    /// <summary>
    /// Adds CORS configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection with CORS configured.</returns>
    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection("CorsSettings");

        services.AddCors(options =>
        {
            options.AddPolicy("ReactFrontendPolicy", policy =>
            {
                var allowedOrigins = corsSettings.GetSection("AllowedOrigins")
                    .Get<string[]>() ?? Array.Empty<string>();

                if (allowedOrigins.Length > 0)
                {
                    policy.WithOrigins(allowedOrigins);
                }
                else
                {
                    // Fallback for development - allow any origin
                    policy.AllowAnyOrigin();
                }

                var allowedMethods = corsSettings.GetSection("AllowedMethods")
                    .Get<string[]>() ?? new[] { "GET", "POST", "PUT", "DELETE", "PATCH", "OPTIONS" };

                policy.WithMethods(allowedMethods);

                var allowedHeaders = corsSettings.GetSection("AllowedHeaders")
                    .Get<string[]>() ?? new[] { "Authorization", "Content-Type", "Accept", "X-Correlation-Id", "X-Request-Id" };

                policy.WithHeaders(allowedHeaders);

                var exposedHeaders = corsSettings.GetSection("ExposedHeaders")
                    .Get<string[]>() ?? new[] { "X-Correlation-Id", "X-Total-Count", "X-Page-Number", "X-Page-Size" };

                policy.WithExposedHeaders(exposedHeaders);

                var allowCredentials = corsSettings.GetValue<bool>("AllowCredentials", true);
                if (allowCredentials)
                {
                    policy.AllowCredentials();
                }

                var maxAge = corsSettings.GetValue<int>("MaxAge", 3600);
                policy.SetPreflightMaxAge(TimeSpan.FromSeconds(maxAge));
            });
        });

        return services;
    }
}