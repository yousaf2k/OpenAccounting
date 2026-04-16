using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Enrichers.Span;
using Customer.Application.Mappings;
using Customer.Application.Commands;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Events;
using Customer.Infrastructure.Repositories;
using BuildingBlocks.EventBus;
using BuildingBlocks.Infrastructure;

var builder = WebApplicationBuilder.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithSpan()
    .WriteTo.Console()
    .WriteTo.Seq(builder.Configuration["Seq:Url"] ?? "http://localhost:5341")
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting Customer Service");

    // Add services to the container
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IVendorRepository, VendorRepository>();
    builder.Services.AddScoped<IContactRepository, ContactRepository>();
    builder.Services.AddScoped<IAddressRepository, AddressRepository>();

    // Register Domain Event Dispatcher (needed by DbContext)
    builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

    // Register DbContext with factory to inject DomainEventDispatcher
    builder.Services.AddScoped<CustomerDbContext>(provider =>
    {
        var options = new DbContextOptionsBuilder<CustomerDbContext>();
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseSqlServer(connectionString, sqlServerOptions =>
        {
            sqlServerOptions.MigrationsAssembly("Customer.Infrastructure");
        });

        var dispatcher = provider.GetRequiredService<IDomainEventDispatcher>();
        return new CustomerDbContext(options.Options, dispatcher);
    });

    // Register UnitOfWork
    builder.Services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<CustomerDbContext>());

    // Register AutoMapper
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    // Register MediatR
    builder.Services.AddMediatR(cfg =>
    {
        cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommand).Assembly);
        cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    });

    // Register FluentValidation
    builder.Services.AddValidatorsFromAssembly(typeof(CreateCustomerCommand).Assembly);

    // Register EventBus
    builder.Services.AddRabbitMQEventBus();

    // Add Authentication
    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.Authority = builder.Configuration["Auth:Authority"] ?? "https://localhost:5001";
            options.TokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        });

    builder.Services.AddAuthorization();

    // Add Swagger
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new()
        {
            Title = "Customer Service API",
            Version = "v1",
            Description = "API for managing customers and vendors"
        });

        // Add JWT authentication to Swagger
        options.AddSecurityDefinition("Bearer", new()
        {
            Type = "OAuth2",
            Flows = new()
            {
                AuthorizationCode = new()
                {
                    AuthorizationUrl = new Uri($"{builder.Configuration["Auth:Authority"]}/connect/authorize"),
                    TokenUrl = new Uri($"{builder.Configuration["Auth:Authority"]}/connect/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        { "openid", "OpenID" },
                        { "profile", "Profile" },
                        { "email", "Email" },
                        { "accounting-api", "Accounting API" }
                    }
                }
            }
        });

        options.AddSecurityRequirement(new()
        {
            {
                new()
                {
                    Reference = new() { Type = "SecurityScheme", Id = "Bearer" }
                },
                new[] { "accounting-api" }
            }
        });

        // Include XML comments
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    });

    // Add health checks
    builder.Services.AddHealthChecks()
        .AddDbContextCheck<CustomerDbContext>();

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Customer Service API V1");
            options.OAuthClientId("accounting-web-app");
            options.OAuthScopes("openid", "profile", "email", "accounting-api", "offline_access");
            options.OAuthUsePkce();
        });
    }

    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health");

    // Apply migrations
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
        await dbContext.Database.MigrateAsync();
        Log.Information("Database migrations completed");
    }

    await app.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

/// <summary>
/// Validation behavior for MediatR pipeline.
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _validators = validators;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning("Validation failed for {RequestType}", typeof(TRequest).Name);
            throw new FluentValidation.ValidationException(failures);
        }

        return await next();
    }
}
