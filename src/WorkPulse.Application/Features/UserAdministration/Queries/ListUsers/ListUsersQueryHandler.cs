using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.UserAdministration.Queries.ListUsers;

public sealed class ListUsersQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListUsersQuery, Result<IReadOnlyList<UserAdministrationUserDto>>>
{
    public async Task<Result<IReadOnlyList<UserAdministrationUserDto>>> Handle(
        ListUsersQuery request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.SystemUsers.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(user =>
                user.UserName.Contains(search)
                || user.Email.Contains(search)
                || user.DisplayName.Contains(search));
        }

        if (request.IsActive is not null)
        {
            query = query.Where(user => user.IsActive == request.IsActive);
        }

        var users = await query
            .OrderBy(user => user.DisplayName)
            .Select(user => new UserAdministrationUserDto(
                user.Id,
                user.IdentityUserId,
                user.UserName,
                user.Email,
                user.DisplayName,
                user.Role,
                user.IsActive))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyList<UserAdministrationUserDto>>.Success(users);
    }
}
