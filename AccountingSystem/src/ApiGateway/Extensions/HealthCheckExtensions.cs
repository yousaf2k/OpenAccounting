using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ApiGateway.HealthChecks;

namespace ApiGateway.Extensions;

/// <summary>
/// Extension methods for configuring health checks.
/// </summary>
public static class HealthCheckExtensions
{
    /// <summary>
    /// Adds health checks configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection with health checks configured.</returns>
    public static IServiceCollection AddHealthChecksConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var healthChecks = services.AddHealthChecks();

        // SQL Server health check
        var sqlConnectionString = configuration.GetValue<string>("HealthChecks:SqlServer:ConnectionString");
        if (!string.IsNullOrEmpty(sqlConnectionString))
        {
            healthChecks.AddSqlServer(
                connectionString: sqlConnectionString,
                name: "SQL Server",
                tags: new[] { "infrastructure", "database" });
        }

        // RabbitMQ health check
        var rabbitMqConnectionString = configuration.GetValue<string>("HealthChecks:RabbitMQ:ConnectionString");
        if (!string.IsNullOrEmpty(rabbitMqConnectionString))
        {
            healthChecks.AddRabbitMQ(
                rabbitMqConnectionString,
                name: "RabbitMQ",
                tags: new[] { "infrastructure", "messagebroker" });
        }

        // Redis health check
        var redisConnectionString = configuration.GetValue<string>("HealthChecks:Redis:ConnectionString");
        if (!string.IsNullOrEmpty(redisConnectionString))
        {
            healthChecks.AddRedis(
                redisConnectionString: redisConnectionString,
                name: "Redis",
                tags: new[] { "infrastructure", "cache" });
        }

        // Service health checks - register ServiceHealthCheck for each downstream service
        var serviceDiscovery = configuration.GetSection("ServiceDiscovery:Services");
        foreach (var service in serviceDiscovery.GetChildren())
        {
            var serviceName = service.Key;
            var serviceUrl = service.Value;

            if (!string.IsNullOrEmpty(serviceUrl))
            {
                healthChecks
                    .AddTypeActivatedCheck<ServiceHealthCheck>(
                        name: $"{serviceName}-health",
                        args: new object[] { serviceUrl, serviceName },
                        failureStatus: HealthStatus.Unhealthy,
                        tags: new[] { "services", "ready", serviceName });
            }
        }

        return services;
    }

    /// <summary>
    /// Maps health check endpoints to the application.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application with health check endpoints mapped.</returns>
    public static WebApplication MapHealthChecks(this WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            AllowCachingResponses = false
        });

        app.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains("ready"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            AllowCachingResponses = false
        });

        app.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = _ => false, // Liveness probe - no specific checks
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            AllowCachingResponses = false
        });

        return app;
    }
}