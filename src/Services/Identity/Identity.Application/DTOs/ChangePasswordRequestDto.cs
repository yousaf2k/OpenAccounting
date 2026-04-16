namespace Identity.Application.DTOs;

/// <summary>
/// DTO for changing password request.
/// </summary>
public class ChangePasswordRequestDto
{
    /// <summary>
    /// Current password.
    /// </summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password confirmation.
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
