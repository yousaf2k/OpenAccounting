using MediatR;
using Identity.Application.DTOs;

namespace Identity.Application.Queries;

/// <summary>
/// Query to get a user by ID.
/// </summary>
public class GetUserByIdQuery : IRequest<UserDto?>
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserByIdQuery"/> class.
    /// </summary>
    public GetUserByIdQuery(Guid userId)
    {
        UserId = userId;
    }
}
