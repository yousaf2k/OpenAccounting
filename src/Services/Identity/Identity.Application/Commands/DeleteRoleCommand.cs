using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Commands;

/// <summary>
/// Command to delete a role (Comment 2).
/// </summary>
public class DeleteRoleCommand : IRequest<MessageDto>
{
    /// <summary>
    /// Role ID to delete.
    /// </summary>
    public Guid Id { get; set; }
}
