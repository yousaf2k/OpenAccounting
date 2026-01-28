using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to revoke all refresh tokens for a user.
/// </summary>
public class RevokeRefreshTokensCommand : IRequest<MessageDto>
{
    /// <summary>
    /// User ID whose tokens should be revoked.
    /// </summary>
    public Guid UserId { get; set; }
}
