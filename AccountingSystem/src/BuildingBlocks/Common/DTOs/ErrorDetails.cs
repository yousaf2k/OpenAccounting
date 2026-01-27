namespace BuildingBlocks.Common.DTOs;

/// <summary>
/// Represents error details for API responses.
/// </summary>
public class ErrorDetails
{
    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the field that caused the error (for validation errors).
    /// </summary>
    public string? Field { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorDetails"/> class.
    /// </summary>
    public ErrorDetails() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorDetails"/> class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="field">The field that caused the error.</param>
    public ErrorDetails(string code, string message, string? field = null)
    {
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Message = message ?? throw new ArgumentNullException(nameof(message));
        Field = field;
    }
}