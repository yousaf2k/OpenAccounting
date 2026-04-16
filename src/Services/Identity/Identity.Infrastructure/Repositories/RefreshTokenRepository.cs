using Identity.Domain.Entities;
using BuildingBlocks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

/// <summary>
/// Repository for managing refresh tokens.
/// </summary>
public class RefreshTokenRepository : Repository<RefreshToken, Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefreshTokenRepository"/> class.
    /// </summary>
    public RefreshTokenRepository(IdentityDbContext context)
        : base(context)
    {
    }

    /// <summary>
    /// Gets a token by its value.
    /// </summary>
    public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token, cancellationToken);
    }

    /// <summary>
    /// Gets all active tokens for a user.
    /// </summary>
    public async Task<List<RefreshToken>> GetActiveTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && !rt.IsExpired)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Revokes all tokens for a user.
    /// </summary>
    public async Task RevokeAllUserTokensAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tokens = await DbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevoked && !rt.IsExpired)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.Revoke();
        }

        await Context.SaveChangesAsync(cancellationToken);
    }
}
