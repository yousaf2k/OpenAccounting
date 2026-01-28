using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Queries;

/// <summary>
/// Query to get a user by email.
/// </summary>
public class GetUserByEmailQuery : IRequest<UserDto?>
{
    /// <summary>
    /// Gets or sets the email.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByEmailQuery"/> class.
    /// </summary>
    public GetUserByEmailQuery(string? email)
    {
        Email = email;
    }
}
