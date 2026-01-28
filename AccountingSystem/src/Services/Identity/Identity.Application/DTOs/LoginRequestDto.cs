namespace Identity.Application.DTOs;

/// <summary>
/// DTO for login request.
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Gets or sets the email or username.
    /// </summary>
    public string? EmailOrUserName { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string? Password { get; set; }
}
