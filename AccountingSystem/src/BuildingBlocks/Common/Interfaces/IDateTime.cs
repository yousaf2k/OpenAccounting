namespace BuildingBlocks.Common.Interfaces;

/// <summary>
/// Abstraction for system time to enable testability.
/// </summary>
public interface IDateTime
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current local date and time.
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Gets the current date.
    /// </summary>
    DateOnly Today { get; }
}