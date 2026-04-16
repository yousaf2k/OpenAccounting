using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace ApiGateway.Extensions;

/// <summary>
/// Extension methods for configuring observability (logging and tracing).
/// </summary>
public static class ObservabilityExtensions
{
    /// <summary>
    /// Adds observability configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection with observability configured.</returns>
    public static IServiceCollection AddObservability(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var openTelemetrySettings = configuration.GetSection("OpenTelemetry");

        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService(
                    serviceName: openTelemetrySettings["ServiceName"] ?? "ApiGateway",
                    serviceVersion: openTelemetrySettings["ServiceVersion"] ?? "1.0.0")
                .AddAttributes(new Dictionary<string, object>
                {
                    ["service.instance.id"] = Environment.MachineName
                }))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.Filter = (httpContext) =>
                        {
                            // Filter out health check endpoints from tracing
                            return !httpContext.Request.Path.Value?.Contains("/health") ?? true;
                        };
                        options.RecordException = true;
                    })
                    .AddSource("ApiGateway.Aggregation");

                var samplingRatio = openTelemetrySettings.GetSection("Tracing").GetValue<double>("SamplingRatio", 1.0);
                tracing.SetSampler(new TraceIdRatioBasedSampler(samplingRatio));

                var jaegerEndpoint = openTelemetrySettings["Jaeger:Endpoint"];
                if (!string.IsNullOrEmpty(jaegerEndpoint))
                {
                    tracing.AddJaegerExporter(options =>
                    {
                        options.Endpoint = new Uri(jaegerEndpoint);
                        options.ExportProcessorType = OpenTelemetry.ExportProcessorType.Batch;
                    });
                }
            });

        return services;
    }

    /// <summary>
    /// Adds Serilog configuration to the web application builder.
    /// </summary>
    /// <param name="builder">The web application builder.</param>
    /// <returns>The web application builder with Serilog configured.</returns>
    public static WebApplicationBuilder AddSerilogConfiguration(this WebApplicationBuilder builder)
    {
        // Configure Serilog from appsettings.json and add correlation ID enrichment
        builder.Host.UseSerilog((context, services, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "ApiGateway")
                // Add correlation ID enrichment
                .Enrich.With(new CorrelationIdEnricher());
        });

        return builder;
    }
}

/// <summary>
/// Serilog enricher for correlation ID.
/// </summary>
public class CorrelationIdEnricher : Serilog.Core.ILogEventEnricher
{
    private const string CorrelationIdPropertyName = "CorrelationId";

    /// <inheritdoc />
    public void Enrich(Serilog.Events.LogEvent logEvent, Serilog.Core.ILogEventPropertyFactory propertyFactory)
    {
        // Try to get correlation ID from async local storage or generate a new one
        var correlationId = System.Diagnostics.Activity.Current?.Id ?? Guid.NewGuid().ToString();

        var correlationIdProperty = propertyFactory.CreateProperty(CorrelationIdPropertyName, correlationId);
        logEvent.AddPropertyIfAbsent(correlationIdProperty);
    }
}