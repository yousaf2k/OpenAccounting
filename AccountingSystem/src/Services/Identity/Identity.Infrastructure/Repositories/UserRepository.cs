using Identity.Domain.Entities;
using BuildingBlocks.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

/// <summary>
/// Repository for managing users.
/// </summary>
public class UserRepository : Repository<ApplicationUser, Guid>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    public UserRepository(IdentityDbContext context)
        : base(context)
    {
    }

    /// <summary>
    /// Gets a user by email.
    /// </summary>
    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant(), cancellationToken);
    }

    /// <summary>
    /// Gets a user by username.
    /// </summary>
    public async Task<ApplicationUser?> GetByUserNameAsync(string userName, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.NormalizedUserName == userName.ToUpperInvariant(), cancellationToken);
    }

    /// <summary>
    /// Gets all active users.
    /// </summary>
    public async Task<List<ApplicationUser>> GetActiveUsersAsync(CancellationToken cancellationToken = default)
    {
        return await DbSet
            .AsNoTracking()
            .Where(u => u.IsActive && !u.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Searches for users.
    /// </summary>
    public async Task<List<ApplicationUser>> SearchUsersAsync(string? searchTerm, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking().Where(u => !u.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var normalized = searchTerm.ToLowerInvariant();
            query = query.Where(u =>
                u.FirstName != null && u.FirstName.ToLower().Contains(normalized) ||
                u.LastName != null && u.LastName.ToLower().Contains(normalized) ||
                u.Email != null && u.Email.ToLower().Contains(normalized) ||
                u.UserName != null && u.UserName.ToLower().Contains(normalized));
        }

        return await query.ToListAsync(cancellationToken);
    }
}
