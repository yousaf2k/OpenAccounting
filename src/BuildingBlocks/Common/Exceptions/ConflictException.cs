namespace BuildingBlocks.Common.Exceptions;

/// <summary>
/// Exception thrown when a conflict occurs (e.g., duplicate entries).
/// </summary>
public class ConflictException : DomainException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    /// <param name="message">The error message describing the conflict.</param>
    public ConflictException(string message)
        : base(message, "CONFLICT")
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConflictException"/> class.
    /// </summary>
    /// <param name="entityName">The name of the entity that caused the conflict.</param>
    /// <param name="propertyName">The name of the property that caused the conflict.</param>
    /// <param name="value">The value that caused the conflict.</param>
    public ConflictException(string entityName, string propertyName, object value)
        : base($"{entityName} with {propertyName} '{value}' already exists.", "CONFLICT")
    {
        EntityName = entityName ?? throw new ArgumentNullException(nameof(entityName));
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets the name of the entity that caused the conflict.
    /// </summary>
    public string? EntityName { get; }

    /// <summary>
    /// Gets the name of the property that caused the conflict.
    /// </summary>
    public string? PropertyName { get; }

    /// <summary>
    /// Gets the value that caused the conflict.
    /// </summary>
    public object? Value { get; }
}