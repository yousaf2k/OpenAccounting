using System.Text.Json;

namespace ApiGateway.Aggregation;

/// <summary>
/// Handles dashboard data aggregation from multiple services.
/// </summary>
public static class DashboardAggregationEndpoint
{
    /// <summary>
    /// Maps the dashboard aggregation endpoint.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application with the endpoint mapped.</returns>
    public static WebApplication MapDashboardAggregation(this WebApplication app)
    {
        app.MapGet("/api/bff/dashboard", async (
            IHttpClientFactory httpClientFactory,
            ILogger logger) =>
        {
            try
            {
                var client = httpClientFactory.CreateClient("AggregatedClient");

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

                return Results.Ok(dashboardData);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error aggregating dashboard data");
                return Results.Problem(
                    detail: "Failed to aggregate dashboard data",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireRateLimiting("Api")
        .RequireAuthorization();

        return app;
    }

    private static async Task<object?> GetCustomerStatsAsync(HttpClient client)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get customer stats: {ex.Message}");
        }
        return new { error = "Customer stats unavailable", totalCustomers = 0, activeCustomers = 0 };
    }

    private static async Task<object?> GetInvoiceStatsAsync(HttpClient client)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get invoice stats: {ex.Message}");
        }
        return new { error = "Invoice stats unavailable", totalInvoices = 0, pendingInvoices = 0 };
    }

    private static async Task<object?> GetPaymentStatsAsync(HttpClient client)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get payment stats: {ex.Message}");
        }
        return new { error = "Payment stats unavailable", totalPayments = 0, pendingPayments = 0 };
    }

    private static async Task<object?> GetGeneralLedgerSummaryAsync(HttpClient client)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get GL summary: {ex.Message}");
        }
        return new { error = "GL summary unavailable", totalAssets = 0, totalLiabilities = 0 };
    }
}