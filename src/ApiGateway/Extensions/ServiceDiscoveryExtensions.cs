using Microsoft.Extensions.ServiceDiscovery;

namespace ApiGateway.Extensions;

/// <summary>
/// Extension methods for configuring service discovery.
/// </summary>
public static class ServiceDiscoveryExtensions
{
    /// <summary>
    /// Adds service discovery configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection with service discovery configured.</returns>
    public static IServiceCollection AddServiceDiscoveryConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configure service discovery for YARP
        services.AddServiceDiscovery();

        // Add HTTP client with service discovery
        services.AddHttpClient("AggregatedClient", client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        });

        return services;
    }
}