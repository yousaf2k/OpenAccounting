namespace BuildingBlocks.Common.Interfaces;

/// <summary>
/// Abstraction for accessing current user context.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>
    /// Gets the current user's identifier.
    /// </summary>
    string? UserId { get; }

    /// <summary>
    /// Gets the current user's name.
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Gets the current user's email address.
    /// </summary>
    string? Email { get; }

    /// <summary>
    /// Gets a value indicating whether the current user is authenticated.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Gets the collection of roles assigned to the current user.
    /// </summary>
    IEnumerable<string> Roles { get; }

    /// <summary>
    /// Gets the collection of claims for the current user.
    /// </summary>
    IEnumerable<KeyValuePair<string, string>> Claims { get; }

    /// <summary>
    /// Checks if the current user has the specified role.
    /// </summary>
    /// <param name="role">The role to check.</param>
    /// <returns>true if the user has the role; otherwise, false.</returns>
    bool IsInRole(string role);

    /// <summary>
    /// Checks if the current user has the specified claim.
    /// </summary>
    /// <param name="claimType">The claim type.</param>
    /// <param name="claimValue">The claim value.</param>
    /// <returns>true if the user has the claim; otherwise, false.</returns>
    bool HasClaim(string claimType, string claimValue);
}