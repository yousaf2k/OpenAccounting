using BuildingBlocks.Common.DTOs;

namespace BuildingBlocks.Common.Exceptions;

/// <summary>
/// Exception thrown when validation fails.
/// </summary>
public class ValidationException : DomainException
{
    /// <summary>
    /// Gets the collection of validation errors.
    /// </summary>
    public IEnumerable<ErrorDetails> Errors { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="errors">The collection of validation errors.</param>
    public ValidationException(IEnumerable<ErrorDetails> errors)
        : base("Validation failed.", "VALIDATION_FAILED")
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="errors">The collection of validation errors.</param>
    public ValidationException(string message, IEnumerable<ErrorDetails> errors)
        : base(message, "VALIDATION_FAILED")
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }
}