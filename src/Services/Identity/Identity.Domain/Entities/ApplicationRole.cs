using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Entities;

/// <summary>
/// Represents a role in the Identity Service.
/// Inherits from IdentityRole&lt;Guid&gt;.
/// </summary>
public class ApplicationRole : IdentityRole<Guid>
{
    /// <summary>
    /// Gets or sets the role description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the permissions as a JSON string.
    /// </summary>
    public string? Permissions { get; set; }
}
