using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to authenticate a user.
/// </summary>
public class LoginCommand : IRequest<LoginResponseDto>
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
