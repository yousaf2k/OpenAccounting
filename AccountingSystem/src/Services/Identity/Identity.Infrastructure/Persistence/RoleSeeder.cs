using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;

namespace Identity.Infrastructure.Persistence;

/// <summary>
/// Seeder for predefined roles with permissions (Comment 2).
/// </summary>
public class RoleSeeder
{
    private readonly RoleManager<ApplicationRole> _roleManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleSeeder"/> class.
    /// </summary>
    public RoleSeeder(RoleManager<ApplicationRole> roleManager)
    {
        _roleManager = roleManager;
    }

    /// <summary>
    /// Seeds predefined roles with permissions.
    /// </summary>
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var roles = new[]
        {
            new
            {
                Name = "Admin",
                Description = "Administrator with full access",
                Permissions = new Dictionary<string, bool>
                {
                    ["user.create"] = true,
                    ["user.read"] = true,
                    ["user.update"] = true,
                    ["user.delete"] = true,
                    ["role.create"] = true,
                    ["role.read"] = true,
                    ["role.update"] = true,
                    ["role.delete"] = true,
                    ["invoice.create"] = true,
                    ["invoice.read"] = true,
                    ["invoice.update"] = true,
                    ["invoice.delete"] = true,
                    ["report.create"] = true,
                    ["report.read"] = true,
                    ["audit.read"] = true,
                },
            },
            new
            {
                Name = "Accountant",
                Description = "Accountant with invoice and report access",
                Permissions = new Dictionary<string, bool>
                {
                    ["user.read"] = true,
                    ["role.read"] = true,
                    ["invoice.create"] = true,
                    ["invoice.read"] = true,
                    ["invoice.update"] = true,
                    ["report.create"] = true,
                    ["report.read"] = true,
                },
            },
            new
            {
                Name = "User",
                Description = "Regular user with basic access",
                Permissions = new Dictionary<string, bool>
                {
                    ["invoice.read"] = true,
                    ["report.read"] = true,
                },
            },
            new
            {
                Name = "Viewer",
                Description = "Read-only viewer",
                Permissions = new Dictionary<string, bool>
                {
                    ["invoice.read"] = true,
                    ["report.read"] = true,
                },
            },
        };

        foreach (var roleData in roles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleData.Name);
            if (roleExists)
            {
                continue;
            }

            var permissionsJson = JsonSerializer.Serialize(roleData.Permissions);

            var role = new ApplicationRole
            {
                Name = roleData.Name,
                Description = roleData.Description,
                Permissions = permissionsJson,
            };

            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Failed to create role '{roleData.Name}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
