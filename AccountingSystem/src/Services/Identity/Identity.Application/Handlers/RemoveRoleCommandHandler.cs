using MediatR;
using Identity.Application.Commands;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Handlers;

/// <summary>
/// Handler for RemoveRoleCommand.
/// </summary>
public class RemoveRoleCommandHandler : IRequestHandler<RemoveRoleCommand, UserDto>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveRoleCommandHandler"/> class.
    /// </summary>
    public RemoveRoleCommandHandler(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    /// <inheritdoc />
    public async Task<UserDto> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {request.UserId} not found");
        }

        var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
        if (!roleExists)
        {
            throw new InvalidOperationException($"Role '{request.RoleName}' does not exist");
        }

        var isInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
        if (!isInRole)
        {
            throw new InvalidOperationException($"User is not in role '{request.RoleName}'");
        }

        var result = await _userManager.RemoveFromRoleAsync(user, request.RoleName);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to remove role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }

        var roles = await _userManager.GetRolesAsync(user);
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsActive = user.IsActive,
            Roles = roles.ToList(),
        };
    }
}
