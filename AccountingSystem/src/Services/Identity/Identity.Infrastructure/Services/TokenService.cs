using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Identity.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure.Services;

/// <summary>
/// Service for generating and validating JWT tokens.
/// </summary>
public class TokenService
{
    private readonly string _signingKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpiryMinutes;
    private readonly int _refreshTokenExpiryDays;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenService"/> class.
    /// </summary>
    public TokenService(
        string signingKey,
        string issuer,
        string audience,
        int accessTokenExpiryMinutes = 60,
        int refreshTokenExpiryDays = 7)
    {
        _signingKey = signingKey ?? throw new ArgumentNullException(nameof(signingKey));
        _issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        _audience = audience ?? throw new ArgumentNullException(nameof(audience));
        _accessTokenExpiryMinutes = accessTokenExpiryMinutes;
        _refreshTokenExpiryDays = refreshTokenExpiryDays;

        if (signingKey.Length < 32)
        {
            throw new ArgumentException("Signing key must be at least 32 characters long", nameof(signingKey));
        }
    }

    /// <summary>
    /// Generates an access token for a user with permissions from roles (Comment 2).
    /// </summary>
    public string GenerateAccessToken(ApplicationUser user, IEnumerable<string> roles, IDictionary<string, object>? rolePermissions = null)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_signingKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
            new Claim("FirstName", user.FirstName ?? string.Empty),
            new Claim("LastName", user.LastName ?? string.Empty),
            new Claim("sub", user.Id.ToString()),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // Add permissions from roles if available (Comment 2)
        if (rolePermissions != null && rolePermissions.Count > 0)
        {
            var permissionsJson = JsonSerializer.Serialize(rolePermissions);
            claims.Add(new Claim("permissions", permissionsJson));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_accessTokenExpiryMinutes),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Generates a refresh token using cryptographically secure random bytes.
    /// </summary>
    public RefreshToken GenerateRefreshToken(Guid userId)
    {
        var randomBuffer = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBuffer);
        }

        var token = Convert.ToBase64String(randomBuffer);

        return new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays),
            CreatedAt = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Validates a JWT token.
    /// </summary>
    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_signingKey);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            return validatedToken is JwtSecurityToken;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Extracts user ID from a token.
    /// </summary>
    public Guid? ExtractUserIdFromToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            return Guid.TryParse(userIdClaim?.Value, out var userId) ? userId : null;
        }
        catch
        {
            return null;
        }
    }
}
