using System.Text.Json;

namespace ApiGateway.Aggregation;

/// <summary>
/// Handles invoice details aggregation from multiple services.
/// </summary>
public static class InvoiceDetailsAggregationEndpoint
{
    /// <summary>
    /// Maps the invoice details aggregation endpoint.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>The web application with the endpoint mapped.</returns>
    public static WebApplication MapInvoiceDetailsAggregation(this WebApplication app)
    {
        app.MapGet("/api/bff/invoice-details", async (
            Guid id,
            IHttpClientFactory httpClientFactory,
            ILogger logger) =>
        {
            try
            {
                var client = httpClientFactory.CreateClient("AggregatedClient");

                // Create tasks for parallel execution
                var invoiceTask = GetInvoiceAsync(client, id);
                var customerTask = GetCustomerAsync(client, id);
                var productsTask = GetInvoiceProductsAsync(client, id);
                var paymentsTask = GetInvoicePaymentsAsync(client, id);

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

                return Results.Ok(invoiceDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error aggregating invoice details for ID: {InvoiceId}", id);
                return Results.Problem(
                    detail: "Failed to aggregate invoice details",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .RequireRateLimiting("Api")
        .RequireAuthorization();

        return app;
    }

    private static async Task<object?> GetInvoiceAsync(HttpClient client, Guid invoiceId)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get invoice {invoiceId}: {ex.Message}");
        }
        return new { error = "Invoice data unavailable", id = invoiceId };
    }

    private static async Task<object?> GetCustomerAsync(HttpClient client, Guid invoiceId)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get customer for invoice {invoiceId}: {ex.Message}");
        }
        return new { error = "Customer data unavailable" };
    }

    private static async Task<object?> GetInvoiceProductsAsync(HttpClient client, Guid invoiceId)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get products for invoice {invoiceId}: {ex.Message}");
        }
        return new { error = "Product data unavailable", products = Array.Empty<object>() };
    }

    private static async Task<object?> GetInvoicePaymentsAsync(HttpClient client, Guid invoiceId)
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
            // Log warning but continue with partial data
            Console.WriteLine($"Failed to get payments for invoice {invoiceId}: {ex.Message}");
        }
        return new { error = "Payment data unavailable", payments = Array.Empty<object>() };
    }
}