namespace Identity.Application.DTOs;

/// <summary>
/// DTO for role data (Comment 2).
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Role ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Role name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Role description.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Role permissions as JSON.
    /// </summary>
    public string? Permissions { get; set; }
}
