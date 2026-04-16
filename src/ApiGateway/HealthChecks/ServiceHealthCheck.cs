using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiGateway.HealthChecks;

/// <summary>
/// Health check for downstream microservices.
/// </summary>
public class ServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _serviceUrl;
    private readonly string _serviceName;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceHealthCheck"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <param name="serviceUrl">The service URL.</param>
    /// <param name="serviceName">The service name.</param>
    public ServiceHealthCheck(
        IHttpClientFactory httpClientFactory,
        string serviceUrl,
        string serviceName)
    {
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _serviceUrl = serviceUrl ?? throw new ArgumentNullException(nameof(serviceUrl));
        _serviceName = serviceName ?? throw new ArgumentNullException(nameof(serviceName));
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var response = await client.GetAsync($"{_serviceUrl}/health", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy($"Service {_serviceName} is healthy");
            }

            return HealthCheckResult.Unhealthy(
                $"Service {_serviceName} returned status code {response.StatusCode}");
        }
        catch (HttpRequestException ex)
        {
            return HealthCheckResult.Unhealthy(
                $"Service {_serviceName} is unreachable: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return HealthCheckResult.Unhealthy(
                $"Service {_serviceName} health check timed out");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                $"Service {_serviceName} health check failed: {ex.Message}");
        }
    }
}