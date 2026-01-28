using System.Text.Json;

namespace ApiGateway.Middleware;

/// <summary>
/// Middleware for request aggregation (Backend for Frontend pattern).
/// </summary>
public class RequestAggregationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RequestAggregationMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestAggregationMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <param name="logger">The logger.</param>
    public RequestAggregationMiddleware(
        RequestDelegate next,
        IHttpClientFactory httpClientFactory,
        ILogger<RequestAggregationMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();

        if (path?.StartsWith("/api/bff/") == true)
        {
            if (path.Contains("/dashboard"))
            {
                await HandleDashboardAggregationAsync(context);
                return;
            }

            if (path.Contains("/invoice-details"))
            {
                await HandleInvoiceDetailsAggregationAsync(context);
                return;
            }
        }

        await _next(context);
    }

    private async Task HandleDashboardAggregationAsync(HttpContext context)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("AggregatedClient");

            // Create tasks for parallel execution
            var customerStatsTask = GetCustomerStatsAsync(client);
            var invoiceStatsTask = GetInvoiceStatsAsync(client);
            var paymentStatsTask = GetPaymentStatsAsync(client);
            var glSummaryTask = GetGeneralLedgerSummaryAsync(client);

            // Wait for all tasks to complete
            await Task.WhenAll(customerStatsTask, invoiceStatsTask, paymentStatsTask, glSummaryTask);

            // Aggregate results
            var dashboardData = new
            {
                customerStats = await customerStatsTask,
                invoiceStats = await invoiceStatsTask,
                paymentStats = await paymentStatsTask,
                generalLedgerSummary = await glSummaryTask,
                timestamp = DateTime.UtcNow
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(dashboardData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aggregating dashboard data");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Failed to aggregate dashboard data",
                message = ex.Message
            });
        }
    }

    private async Task HandleInvoiceDetailsAggregationAsync(HttpContext context)
    {
        try
        {
            var invoiceId = context.Request.Query["id"].ToString();
            if (string.IsNullOrEmpty(invoiceId))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Invoice ID is required" });
                return;
            }

            var client = _httpClientFactory.CreateClient("AggregatedClient");

            // Create tasks for parallel execution
            var invoiceTask = GetInvoiceAsync(client, invoiceId);
            var customerTask = GetCustomerAsync(client, invoiceId);
            var productsTask = GetInvoiceProductsAsync(client, invoiceId);
            var paymentsTask = GetInvoicePaymentsAsync(client, invoiceId);

            // Wait for all tasks to complete
            await Task.WhenAll(invoiceTask, customerTask, productsTask, paymentsTask);

            // Aggregate results
            var invoiceDetails = new
            {
                invoice = await invoiceTask,
                customer = await customerTask,
                products = await productsTask,
                payments = await paymentsTask,
                timestamp = DateTime.UtcNow
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(invoiceDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error aggregating invoice details for ID: {InvoiceId}", context.Request.Query["id"]);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Failed to aggregate invoice details",
                message = ex.Message
            });
        }
    }

    private async Task<object?> GetCustomerStatsAsync(HttpClient client)
    {
        try
        {
            var response = await client.GetAsync("http://customer-service/api/customers/stats");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get customer stats");
        }
        return new { error = "Customer stats unavailable" };
    }

    private async Task<object?> GetInvoiceStatsAsync(HttpClient client)
    {
        try
        {
            var response = await client.GetAsync("http://invoice-service/api/invoices/stats");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get invoice stats");
        }
        return new { error = "Invoice stats unavailable" };
    }

    private async Task<object?> GetPaymentStatsAsync(HttpClient client)
    {
        try
        {
            var response = await client.GetAsync("http://payment-service/api/payments/stats");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get payment stats");
        }
        return new { error = "Payment stats unavailable" };
    }

    private async Task<object?> GetGeneralLedgerSummaryAsync(HttpClient client)
    {
        try
        {
            var response = await client.GetAsync("http://gl-service/api/gl/summary");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get GL summary");
        }
        return new { error = "GL summary unavailable" };
    }

    private async Task<object?> GetInvoiceAsync(HttpClient client, string invoiceId)
    {
        try
        {
            var response = await client.GetAsync($"http://invoice-service/api/invoices/{invoiceId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get invoice {InvoiceId}", invoiceId);
        }
        return new { error = "Invoice data unavailable" };
    }

    private async Task<object?> GetCustomerAsync(HttpClient client, string invoiceId)
    {
        try
        {
            var response = await client.GetAsync($"http://invoice-service/api/invoices/{invoiceId}/customer");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get customer for invoice {InvoiceId}", invoiceId);
        }
        return new { error = "Customer data unavailable" };
    }

    private async Task<object?> GetInvoiceProductsAsync(HttpClient client, string invoiceId)
    {
        try
        {
            var response = await client.GetAsync($"http://invoice-service/api/invoices/{invoiceId}/products");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get products for invoice {InvoiceId}", invoiceId);
        }
        return new { error = "Product data unavailable" };
    }

    private async Task<object?> GetInvoicePaymentsAsync(HttpClient client, string invoiceId)
    {
        try
        {
            var response = await client.GetAsync($"http://payment-service/api/payments/invoice/{invoiceId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(content);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to get payments for invoice {InvoiceId}", invoiceId);
        }
        return new { error = "Payment data unavailable" };
    }
}