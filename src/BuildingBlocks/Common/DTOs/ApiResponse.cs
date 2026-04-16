namespace BuildingBlocks.Common.DTOs;

/// <summary>
/// Represents a standard API response wrapper.
/// </summary>
/// <typeparam name="T">The type of the response data.</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Gets or sets a value indicating whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the response data.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Gets or sets a message describing the result of the operation.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Gets or sets a collection of error details.
    /// </summary>
    public IEnumerable<ErrorDetails>? Errors { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
    /// </summary>
    public ApiResponse() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class with success status.
    /// </summary>
    /// <param name="data">The response data.</param>
    /// <param name="message">A message describing the result.</param>
    public ApiResponse(T data, string? message = null)
    {
        Success = true;
        Data = data;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class with error status.
    /// </summary>
    /// <param name="message">A message describing the error.</param>
    /// <param name="errors">A collection of error details.</param>
    public ApiResponse(string message, IEnumerable<ErrorDetails>? errors = null)
    {
        Success = false;
        Message = message;
        Errors = errors;
    }

    /// <summary>
    /// Creates a successful API response.
    /// </summary>
    /// <param name="data">The response data.</param>
    /// <param name="message">A message describing the result.</param>
    /// <returns>A successful API response.</returns>
    public static ApiResponse<T> SuccessResponse(T data, string? message = null)
    {
        return new ApiResponse<T>(data, message);
    }

    /// <summary>
    /// Creates an error API response.
    /// </summary>
    /// <param name="message">A message describing the error.</param>
    /// <param name="errors">A collection of error details.</param>
    /// <returns>An error API response.</returns>
    public static ApiResponse<T> ErrorResponse(string message, IEnumerable<ErrorDetails>? errors = null)
    {
        return new ApiResponse<T>(message, errors);
    }
}