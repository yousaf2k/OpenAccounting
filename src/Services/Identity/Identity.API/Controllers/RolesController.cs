using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Identity.Application.Commands;
using Identity.Application.DTOs;
using Identity.Application.Queries;

namespace Identity.API.Controllers;

/// <summary>
/// Controller for role management endpoints (Comment 2).
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOnly")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RolesController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RolesController"/> class.
    /// </summary>
    public RolesController(IMediator mediator, ILogger<RolesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Gets all roles (Comment 2).
    /// </summary>
    /// <param name="pageNumber">Page number.</param>
    /// <param name="pageSize">Page size.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of roles.</returns>
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAll(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var query = new GetAllRolesQuery { PageNumber = pageNumber, PageSize = pageSize };
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a role by ID (Comment 2).
    /// </summary>
    /// <param name="id">Role ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Role details.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRoleByIdQuery(id);
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Creates a new role (Comment 2).
    /// </summary>
    /// <param name="request">Create role request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Created role details.</returns>
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    /// <summary>
    /// Updates an existing role (Comment 2).
    /// </summary>
    /// <param name="id">Role ID to update.</param>
    /// <param name="request">Update role request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Updated role details.</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> Update(Guid id, UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        request.Id = id;
        var result = await _mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a role (Comment 2).
    /// </summary>
    /// <param name="id">Role ID to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Status message.</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<MessageDto>> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteRoleCommand { Id = id };
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
