using MediatR;
using Identity.Application.DTOs;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Handlers;

/// <summary>
/// Handler for GetAllRolesQuery (Comment 2).
/// </summary>
public class GetAllRolesQueryHandler : IRequestHandler<Queries.GetAllRolesQuery, List<RoleDto>>
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetAllRolesQueryHandler"/> class.
    /// </summary>
    public GetAllRolesQueryHandler(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <inheritdoc />
    public async Task<List<RoleDto>> Handle(Queries.GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = _roleManager.Roles
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name!,
            Description = role.Description,
            Permissions = role.Permissions,
        }).ToList();
    }
}
