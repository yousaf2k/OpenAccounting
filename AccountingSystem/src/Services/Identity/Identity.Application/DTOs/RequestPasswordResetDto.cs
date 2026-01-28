namespace Identity.Application.DTOs;

/// <summary>
/// DTO for requesting password reset.
/// </summary>
public class RequestPasswordResetDto
{
    /// <summary>
    /// User email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;
}
