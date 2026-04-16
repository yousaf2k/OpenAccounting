using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiGateway.Extensions;

/// <summary>
/// Extension methods for configuring authentication and authorization.
/// </summary>
public static class AuthenticationExtensions
{
    /// <summary>
    /// Adds authentication configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The service collection with authentication configured.</returns>
    public static IServiceCollection AddAuthenticationConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var authSettings = configuration.GetSection("Authentication");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var jwtSettings = authSettings.GetSection("Schemes").GetSection("Bearer");

            options.Authority = jwtSettings["ValidIssuer"];
            options.Audience = string.Join(",", jwtSettings.GetSection("ValidAudiences").Get<string[]>());
            options.RequireHttpsMetadata = false; // Set to true in production

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = jwtSettings.GetValue<bool>("ValidateIssuer", true),
                ValidateAudience = jwtSettings.GetValue<bool>("ValidateAudience", true),
                ValidateLifetime = jwtSettings.GetValue<bool>("ValidateLifetime", true),
                ValidateIssuerSigningKey = jwtSettings.GetValue<bool>("ValidateIssuerSigningKey", true),
                ValidIssuer = jwtSettings["ValidIssuer"],
                ValidAudiences = jwtSettings.GetSection("ValidAudiences").Get<string[]>(),
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["IssuerSigningKey"] ?? "default-key-change-in-production"))
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                },
                OnTokenValidated = context =>
                {
                    Console.WriteLine($"Token validated for user: {context.Principal?.Identity?.Name}");
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

    /// <summary>
    /// Adds authorization configuration to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection with authorization configured.</returns>
    public static IServiceCollection AddAuthorizationConfiguration(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            // Role-based policies
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("Admin"));

            options.AddPolicy("AccountantOnly", policy =>
                policy.RequireRole("Admin", "Accountant"));

            options.AddPolicy("UserAccess", policy =>
                policy.RequireRole("Admin", "Accountant", "User"));

            options.AddPolicy("ViewerAccess", policy =>
                policy.RequireRole("Admin", "Accountant", "User", "Viewer"));

            // Claim-based policies
            options.AddPolicy("CanCreateInvoice", policy =>
                policy.RequireClaim("permission", "invoice.create"));

            options.AddPolicy("CanApprovePayments", policy =>
                policy.RequireClaim("permission", "payment.approve"));

            options.AddPolicy("CanViewReports", policy =>
                policy.RequireClaim("permission", "reports.view"));

            options.AddPolicy("CanManageUsers", policy =>
                policy.RequireClaim("permission", "users.manage"));
        });

        return services;
    }
}