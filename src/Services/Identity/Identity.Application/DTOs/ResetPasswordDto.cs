namespace Identity.Application.DTOs;

/// <summary>
/// DTO for resetting password with token.
/// </summary>
public class ResetPasswordDto
{
    /// <summary>
    /// User email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Password reset token.
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// New password.
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// New password confirmation.
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
