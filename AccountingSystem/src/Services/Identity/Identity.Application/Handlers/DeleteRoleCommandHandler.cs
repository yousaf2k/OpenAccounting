using MediatR;
using Identity.Application.Commands;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Handlers;

/// <summary>
/// Handler for DeleteRoleCommand (Comment 2).
/// </summary>
public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, MessageDto>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteRoleCommandHandler"/> class.
    /// </summary>
    public DeleteRoleCommandHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <inheritdoc />
    public async Task<MessageDto> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
        {
            throw new KeyNotFoundException($"Role with ID {request.Id} not found");
        }

        // Prevent deletion of system roles
        if (role.Name == "Admin" || role.Name == "Accountant" || role.Name == "User" || role.Name == "Viewer")
        {
            throw new InvalidOperationException($"Cannot delete system role '{role.Name}'");
        }

        var result = await _roleManager.DeleteAsync(role);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to delete role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        return new MessageDto { Message = $"Role '{role.Name}' deleted successfully" };
    }
}
