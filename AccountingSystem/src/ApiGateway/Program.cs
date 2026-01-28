using ApiGateway.Aggregation;
using ApiGateway.Configuration;
using ApiGateway.Extensions;
using ApiGateway.Middleware;
using BuildingBlocks.Common.Exceptions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add Serilog configuration
builder.AddSerilogConfiguration();

// Add services to the container
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

// Add rate limiting
builder.Services.AddRateLimitingConfiguration(builder.Configuration);

// Add CORS
builder.Services.AddCorsConfiguration(builder.Configuration);

// Add health checks
builder.Services.AddHealthChecksConfiguration(builder.Configuration);

// Add authentication and authorization
builder.Services.AddAuthenticationConfiguration(builder.Configuration);
builder.Services.AddAuthorizationConfiguration();

// Add service discovery
builder.Services.AddServiceDiscoveryConfiguration(builder.Configuration);

// Add YARP reverse proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddServiceDiscoveryDestinationResolver();

// Add observability (OpenTelemetry)
builder.Services.AddObservability(builder.Configuration);

// Add custom middleware services
builder.Services.AddScoped<CorrelationIdMiddleware>();
builder.Services.AddScoped<RequestAggregationMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Add standard middleware
app.UseSerilogRequestLogging();

// Configure CORS
app.UseCors("ReactFrontendPolicy");

// Add correlation ID middleware early
app.UseMiddleware<CorrelationIdMiddleware>();

// Configure authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Configure rate limiting
app.UseRateLimiter();

// Add request aggregation middleware after auth and rate limiting
app.UseMiddleware<RequestAggregationMiddleware>();

// Map health check endpoints
app.MapHealthChecks();

// Map aggregation endpoints
app.MapDashboardAggregation();
app.MapInvoiceDetailsAggregation();

// Map reverse proxy routes with rate limiting (must be last)
app.MapReverseProxy().RequireRateLimiting("Global");

app.Run();