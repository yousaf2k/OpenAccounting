namespace Identity.Application.DTOs;

/// <summary>
/// DTO for assigning a role to a user.
/// </summary>
public class AssignRoleRequestDto
{
    /// <summary>
    /// Role name to assign.
    /// </summary>
    public string RoleName { get; set; } = string.Empty;
}
