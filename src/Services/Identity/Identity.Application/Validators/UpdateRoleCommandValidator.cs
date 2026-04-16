using FluentValidation;
using Identity.Application.Commands;

namespace Identity.Application.Validators;

/// <summary>
/// Validator for UpdateRoleCommand (Comment 2).
/// </summary>
public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateRoleCommandValidator"/> class.
    /// </summary>
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Role ID is required");

        RuleFor(x => x.Name)
            .MinimumLength(3).WithMessage("Role name must be at least 3 characters")
            .MaximumLength(256).WithMessage("Role name must not exceed 256 characters")
            .When(x => !string.IsNullOrEmpty(x.Name));

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Description));

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
