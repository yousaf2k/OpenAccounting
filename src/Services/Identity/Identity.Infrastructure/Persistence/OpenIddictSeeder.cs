using OpenIddict.Abstractions;
using OpenIddict.Core;

namespace Identity.Infrastructure.Persistence;

/// <summary>
/// Seeder for OpenIddict applications and scopes (Comment 1).
/// </summary>
public class OpenIddictSeeder
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly IOpenIddictScopeManager _scopeManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="OpenIddictSeeder"/> class.
    /// </summary>
    public OpenIddictSeeder(
        IOpenIddictApplicationManager applicationManager,
        IOpenIddictScopeManager scopeManager)
    {
        _applicationManager = applicationManager;
        _scopeManager = scopeManager;
    }

    /// <summary>
    /// Seeds OpenIddict scopes and applications.
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await SeedScopesAsync(cancellationToken);
        await SeedApplicationsAsync(cancellationToken);
    }

    private async Task SeedScopesAsync(CancellationToken cancellationToken)
    {
        var scopes = new[]
        {
            "openid",
            "profile",
            "email",
            "accounting-api",
            "offline_access",
        };

        foreach (var scopeName in scopes)
        {
            var scope = await _scopeManager.FindByNameAsync(scopeName, cancellationToken);
            if (scope != null)
            {
                continue;
            }

            var descriptor = new OpenIddictScopeDescriptor
            {
                Name = scopeName,
                DisplayName = scopeName switch
                {
                    "accounting-api" => "Accounting API",
                    "offline_access" => "Offline Access",
                    _ => scopeName,
                },
                Resources =
                {
                    "accounting-api",
                },
            };

            await _scopeManager.CreateAsync(descriptor, cancellationToken);
        }
    }

    private async Task SeedApplicationsAsync(CancellationToken cancellationToken)
    {
        // Seed accounting-web-app (Public PKCE client for React SPA)
        var webAppClient = await _applicationManager.FindByClientIdAsync(
            "accounting-web-app", cancellationToken);

        if (webAppClient == null)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = "accounting-web-app",
                DisplayName = "Accounting Web App",
                ClientType = OpenIddictConstants.ClientTypes.Public, // Public for PKCE
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.OpenId,
                    $"{OpenIddictConstants.Permissions.Prefixes.Scope}accounting-api",
                    OpenIddictConstants.Permissions.Prefixes.Scope + "offline_access",
                },
                PostLogoutRedirectUris =
                {
                    new Uri("http://localhost:3000"),
                    new Uri("http://localhost:5173"),
                },
                RedirectUris =
                {
                    new Uri("http://localhost:3000/callback"),
                    new Uri("http://localhost:5173/callback"),
                },
                Requirements =
                {
                    OpenIddictConstants.Requirements.ProofKeyForCodeExchange,
                },
            };

            await _applicationManager.CreateAsync(descriptor, cancellationToken);
        }

        // Seed accounting-api-gateway (Confidential client for service-to-service)
        var apiGatewayClient = await _applicationManager.FindByClientIdAsync(
            "accounting-api-gateway", cancellationToken);

        if (apiGatewayClient == null)
        {
            var descriptor = new OpenIddictApplicationDescriptor
            {
                ClientId = "accounting-api-gateway",
                ClientSecret = "accounting-api-gateway-secret", // TODO: Use secure secret management
                DisplayName = "Accounting API Gateway",
                ClientType = OpenIddictConstants.ClientTypes.Confidential,
                Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    $"{OpenIddictConstants.Permissions.Prefixes.Scope}accounting-api",
                },
            };

            await _applicationManager.CreateAsync(descriptor, cancellationToken);
        }
    }
}
