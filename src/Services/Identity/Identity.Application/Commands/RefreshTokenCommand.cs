using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to refresh an access token.
/// </summary>
public class RefreshTokenCommand : IRequest<TokenResponseDto>
{
    /// <summary>
    /// Gets or sets the refresh token.
    /// </summary>
    public string? RefreshToken { get; set; }
}
