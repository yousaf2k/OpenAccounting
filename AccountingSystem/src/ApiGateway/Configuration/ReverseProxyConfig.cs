using Yarp.ReverseProxy.Configuration;

namespace ApiGateway.Configuration;

/// <summary>
/// Configuration for YARP Reverse Proxy routes and clusters.
/// </summary>
public static class ReverseProxyConfig
{
    /// <summary>
    /// Gets the route configurations for all microservices.
    /// </summary>
    /// <returns>A collection of route configurations.</returns>
    public static IReadOnlyList<RouteConfig> GetRoutes()
    {
        return new List<RouteConfig>
        {
            new RouteConfig
            {
                RouteId = "identity-route",
                ClusterId = "identity-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/identity/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/identity" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/identity" }
                }
            },
            new RouteConfig
            {
                RouteId = "customers-route",
                ClusterId = "customers-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/customers/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/customers" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/customers" }
                }
            },
            new RouteConfig
            {
                RouteId = "products-route",
                ClusterId = "products-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/products/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/products" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/products" }
                }
            },
            new RouteConfig
            {
                RouteId = "invoices-route",
                ClusterId = "invoices-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/invoices/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/invoices" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/invoices" }
                }
            },
            new RouteConfig
            {
                RouteId = "payments-route",
                ClusterId = "payments-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/payments/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/payments" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/payments" }
                }
            },
            new RouteConfig
            {
                RouteId = "gl-route",
                ClusterId = "gl-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/gl/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/gl" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/gl" }
                }
            },
            new RouteConfig
            {
                RouteId = "ar-route",
                ClusterId = "ar-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/ar/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/ar" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/ar" }
                }
            },
            new RouteConfig
            {
                RouteId = "ap-route",
                ClusterId = "ap-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/ap/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/ap" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/ap" }
                }
            },
            new RouteConfig
            {
                RouteId = "reports-route",
                ClusterId = "reports-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/reports/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/reports" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/reports" }
                }
            },
            new RouteConfig
            {
                RouteId = "documents-route",
                ClusterId = "documents-cluster",
                Match = new RouteMatch
                {
                    Path = "/api/documents/{**catch-all}"
                },
                Transforms = new List<Dictionary<string, string>>
                {
                    new() { ["PathRemovePrefix"] = "/api/documents" },
                    new() { ["RequestHeaderXForwardedPrefix"] = "/api/documents" }
                }
            }
        };
    }

    /// <summary>
    /// Gets the cluster configurations for all microservices.
    /// </summary>
    /// <returns>A collection of cluster configurations.</returns>
    public static IReadOnlyList<ClusterConfig> GetClusters()
    {
        return new List<ClusterConfig>
        {
            new ClusterConfig
            {
                ClusterId = "identity-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["identity-service"] = new DestinationConfig { Address = "http://identity-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "customers-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["customer-service"] = new DestinationConfig { Address = "http://customer-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "products-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["product-service"] = new DestinationConfig { Address = "http://product-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "invoices-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["invoice-service"] = new DestinationConfig { Address = "http://invoice-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "payments-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["payment-service"] = new DestinationConfig { Address = "http://payment-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "gl-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["gl-service"] = new DestinationConfig { Address = "http://gl-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "ar-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["ar-service"] = new DestinationConfig { Address = "http://ar-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "ap-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["ap-service"] = new DestinationConfig { Address = "http://ap-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "reports-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["reporting-service"] = new DestinationConfig { Address = "http://reporting-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            },
            new ClusterConfig
            {
                ClusterId = "documents-cluster",
                Destinations = new Dictionary<string, DestinationConfig>
                {
                    ["document-service"] = new DestinationConfig { Address = "http://document-service" }
                },
                HealthCheck = new HealthCheckConfig
                {
                    Active = new ActiveHealthCheckConfig
                    {
                        Enabled = true,
                        Interval = TimeSpan.FromSeconds(30),
                        Timeout = TimeSpan.FromSeconds(10),
                        Policy = "ConsecutiveFailures",
                        Path = "/health"
                    },
                    Passive = new PassiveHealthCheckConfig
                    {
                        Enabled = true,
                        Policy = "TransportFailureRate",
                        ReactivationPeriod = TimeSpan.FromMinutes(5)
                    }
                },
                LoadBalancingPolicy = "RoundRobin"
            }
        };
    }
}
