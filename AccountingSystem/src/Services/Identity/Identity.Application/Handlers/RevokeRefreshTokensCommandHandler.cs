using MediatR;
using Identity.Application.Commands;
using Identity.Application.DTOs;
using Identity.Infrastructure.Repositories;

namespace Identity.Application.Handlers;

/// <summary>
/// Handler for RevokeRefreshTokensCommand.
/// </summary>
public class RevokeRefreshTokensCommandHandler : IRequestHandler<RevokeRefreshTokensCommand, MessageDto>
{
    private readonly RefreshTokenRepository _refreshTokenRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="RevokeRefreshTokensCommandHandler"/> class.
    /// </summary>
    public RevokeRefreshTokensCommandHandler(RefreshTokenRepository refreshTokenRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
    }

    /// <inheritdoc />
    public async Task<MessageDto> Handle(RevokeRefreshTokensCommand request, CancellationToken cancellationToken)
    {
        await _refreshTokenRepository.RevokeAllUserTokensAsync(request.UserId, cancellationToken);
        await _refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return new MessageDto { Message = "All refresh tokens revoked successfully" };
    }
}
