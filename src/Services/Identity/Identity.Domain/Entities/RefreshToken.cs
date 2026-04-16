using BuildingBlocks.Common.Entities;

namespace Identity.Domain.Entities;

/// <summary>
/// Represents a refresh token for a user.
/// </summary>
public class RefreshToken : Entity<Guid>
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the token value.
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Gets or sets the token expiration date.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the creation date.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the revocation date, if revoked.
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Gets or sets the ID of the token that replaced this one.
    /// </summary>
    public Guid? ReplacedByToken { get; set; }

    /// <summary>
    /// Gets a value indicating whether the token is expired.
    /// </summary>
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    /// <summary>
    /// Gets a value indicating whether the token is revoked.
    /// </summary>
    public bool IsRevoked => RevokedAt.HasValue;

    /// <summary>
    /// Gets a value indicating whether the token is active.
    /// </summary>
    public bool IsActive => !IsExpired && !IsRevoked;

    /// <summary>
    /// Revokes the token.
    /// </summary>
    public void Revoke()
    {
        RevokedAt = DateTime.UtcNow;
    }
}
