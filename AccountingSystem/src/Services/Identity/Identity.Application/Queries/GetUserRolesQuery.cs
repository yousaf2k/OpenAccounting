using MediatR;

namespace Identity.Application.Queries;

/// <summary>
/// Query to get user roles.
/// </summary>
public class GetUserRolesQuery : IRequest<List<string>>
{
    /// <summary>
    /// Gets or sets the user ID.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="GetUserRolesQuery"/> class.
    /// </summary>
    public GetUserRolesQuery(Guid userId)
    {
        UserId = userId;
    }
}
