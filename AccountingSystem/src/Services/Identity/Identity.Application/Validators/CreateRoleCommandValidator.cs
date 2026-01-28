using FluentValidation;
using Identity.Application.Commands;

namespace Identity.Application.Validators;

/// <summary>
/// Validator for CreateRoleCommand (Comment 2).
/// </summary>
public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateRoleCommandValidator"/> class.
    /// </summary>
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Role name is required")
            .MinimumLength(3).WithMessage("Role name must be at least 3 characters")
            .MaximumLength(256).WithMessage("Role name must not exceed 256 characters");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters");

        RuleFor(x => x.Permissions)
            .Must(ValidatePermissionsJson)
            .WithMessage("Permissions must be valid JSON")
            .When(x => !string.IsNullOrEmpty(x.Permissions));
    }

    private bool ValidatePermissionsJson(string? permissions)
    {
        if (string.IsNullOrEmpty(permissions))
        {
            return true;
        }

        try
        {
            System.Text.Json.JsonDocument.Parse(permissions);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
