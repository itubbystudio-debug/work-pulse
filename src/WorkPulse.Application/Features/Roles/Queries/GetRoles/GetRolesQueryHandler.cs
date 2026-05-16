using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Roles.Queries.GetRoles;

public sealed class GetRolesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetRolesQuery, Result<IReadOnlyCollection<RoleListItemDto>>>
{
    public async Task<Result<IReadOnlyCollection<RoleListItemDto>>> Handle(
        GetRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await dbContext.Roles
            .AsNoTracking()
            .OrderBy(role => role.Name)
            .Select(role => new RoleListItemDto(
                role.Id,
                role.Code,
                role.Name,
                role.Description,
                role.IsActive))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<RoleListItemDto>>.Success(roles);
    }
}
