using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Commands;
using Identity.Application.DTOs;
using Identity.Application.Queries;

namespace Identity.API.Controllers;

/// <summary>
/// Controller for user management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of users.</returns>
    [HttpGet]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<List<UserDto>>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetAllUsersQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a user by ID.
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User details.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Gets the current user.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Current user details.</returns>
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (!Guid.TryParse(userId, out var parsedId))
        {
            return Unauthorized();
        }

        var query = new GetUserByIdQuery(parsedId);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Searches for users.
    /// </summary>
    /// <param name="searchTerm">Search term.</param>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of matching users.</returns>
    [HttpGet("search")]
    [Authorize(Roles = "Admin,Accountant")]
    public async Task<ActionResult<List<string>>> Search(string? searchTerm = null, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new SearchUsersQuery(searchTerm) { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Activates a user (Comment 4).
    /// </summary>
    /// <param name="id">User ID to activate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated user details.</returns>
    [HttpPost("{id}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> ActivateUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new ActivateUserCommand { UserId = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deactivates a user (Comment 4).
    /// </summary>
    /// <param name="id">User ID to deactivate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated user details.</returns>
    [HttpPost("{id}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> DeactivateUser(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeactivateUserCommand { UserId = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Assigns a role to a user (Comment 4).
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="request">Role assignment request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated user details.</returns>
    [HttpPost("{id}/roles/assign")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> AssignRole(Guid id, AssignRoleRequestDto request, CancellationToken cancellationToken)
    {
        var command = new AssignRoleCommand { UserId = id, RoleName = request.RoleName };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Removes a role from a user (Comment 4).
    /// </summary>
    /// <param name="id">User ID.</param>
    /// <param name="request">Role removal request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated user details.</returns>
    [HttpPost("{id}/roles/remove")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<UserDto>> RemoveRole(Guid id, RemoveRoleRequestDto request, CancellationToken cancellationToken)
    {
        var command = new RemoveRoleCommand { UserId = id, RoleName = request.RoleName };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
