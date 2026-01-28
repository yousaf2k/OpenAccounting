using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Commands;
using Identity.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using Identity.Domain.Entities;

namespace Identity.API.Controllers;

/// <summary>
/// Controller for authentication endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    private readonly UserManager<ApplicationUser> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    public AuthController(
        IMediator mediator,
        ILogger<AuthController> logger,
        UserManager<ApplicationUser> userManager)
    {
        _mediator = mediator;
        _logger = logger;
        _userManager = userManager;
    }

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">Registration request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User details.</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Register(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand
        {
            Email = request.Email,
            UserName = request.UserName,
            Password = request.Password,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
        };

        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(Register), result);
    }

    /// <summary>
    /// Authenticates a user and returns tokens.
    /// </summary>
    /// <param name="request">Login request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Access token and refresh token.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto request, CancellationToken cancellationToken)
    {
        var command = new LoginCommand
        {
            EmailOrUserName = request.EmailOrUserName,
            Password = request.Password,
        };

        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Refreshes the access token.
    /// </summary>
    /// <param name="request">Refresh token request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>New access token and refresh token.</returns>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<TokenResponseDto>> Refresh(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Changes the password for the authenticated user (Comment 4).
    /// </summary>
    /// <param name="request">Change password request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Status message.</returns>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<ActionResult<MessageDto>> ChangePassword(ChangePasswordRequestDto request, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;
        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            return Unauthorized("Invalid token");
        }

        var user = await _userManager.FindByIdAsync(parsedUserId.ToString());
        if (user == null)
        {
            return NotFound("User not found");
        }

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new MessageDto { Message = "Password changed successfully" });
    }

    /// <summary>
    /// Requests a password reset (Comment 4).
    /// </summary>
    /// <param name="request">Password reset request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Status message.</returns>
    [HttpPost("request-password-reset")]
    [AllowAnonymous]
    public async Task<ActionResult<MessageDto>> RequestPasswordReset(RequestPasswordResetDto request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            // Return success to avoid user enumeration
            return Ok(new MessageDto { Message = "If an account with that email exists, a password reset link will be sent" });
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        // In a real application, send this token via email
        _logger.LogInformation("Password reset token generated for user {UserId}: {Token}", user.Id, token);

        return Ok(new MessageDto { Message = "If an account with that email exists, a password reset link will be sent" });
    }

    /// <summary>
    /// Resets the password using a reset token (Comment 4).
    /// </summary>
    /// <param name="request">Password reset request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Status message.</returns>
    [HttpPost("reset-password")]
    [AllowAnonymous]
    public async Task<ActionResult<MessageDto>> ResetPassword(ResetPasswordDto request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return BadRequest("Invalid user email");
        }

        var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!result.Succeeded)
        {
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        return Ok(new MessageDto { Message = "Password reset successfully" });
    }

    /// <summary>
    /// Logs out the authenticated user by revoking refresh tokens (Comment 4).
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Status message.</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<MessageDto>> Logout(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;
        if (!Guid.TryParse(userId, out var parsedUserId))
        {
            return Unauthorized("Invalid token");
        }

        // In a real application, revoke all refresh tokens for this user
        var command = new RevokeRefreshTokensCommand { UserId = parsedUserId };
        await _mediator.Send(command, cancellationToken);

        return Ok(new MessageDto { Message = "Logged out successfully" });
    }
}
