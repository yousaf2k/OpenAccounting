namespace Identity.Application.DTOs;

/// <summary>
/// DTO for removing a role from a user.
/// </summary>
public class RemoveRoleRequestDto
{
    /// <summary>
    /// Role name to remove.
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
}
