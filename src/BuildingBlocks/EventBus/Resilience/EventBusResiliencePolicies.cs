using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace BuildingBlocks.EventBus.Resilience;

/// <summary>
/// Shared resilience policies for event bus operations.
/// </summary>
public static class EventBusResiliencePolicies
{
    /// <summary>
    /// Gets the retry policy for event publishing.
    /// </summary>
    /// <param name="retryCount">The number of retries.</param>
    /// <returns>The retry policy.</returns>
    public static AsyncRetryPolicy GetRetryPolicy(int retryCount = 3)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    /// <summary>
    /// Gets the circuit breaker policy for event publishing.
    /// </summary>
    /// <param name="failureThreshold">The failure threshold.</param>
    /// <param name="recoveryTime">The recovery time.</param>
    /// <returns>The circuit breaker policy.</returns>
    public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(int failureThreshold = 5, TimeSpan? recoveryTime = null)
    {
        return Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(failureThreshold, recoveryTime ?? TimeSpan.FromMinutes(1));
    }

    /// <summary>
    /// Gets a combined policy with both retry and circuit breaker.
    /// </summary>
    /// <param name="retryCount">The number of retries.</param>
    /// <param name="failureThreshold">The failure threshold.</param>
    /// <param name="recoveryTime">The recovery time.</param>
    /// <returns>The combined policy.</returns>
    public static AsyncPolicy GetCombinedPolicy(int retryCount = 3, int failureThreshold = 5, TimeSpan? recoveryTime = null)
    {
        var retryPolicy = GetRetryPolicy(retryCount);
        var circuitBreakerPolicy = GetCircuitBreakerPolicy(failureThreshold, recoveryTime);

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }
}