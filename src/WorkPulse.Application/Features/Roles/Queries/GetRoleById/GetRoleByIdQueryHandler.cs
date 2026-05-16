using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Roles.Queries.GetRoleById;

public sealed class GetRoleByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetRoleByIdQuery, Result<RoleDto>>
{
    public async Task<Result<RoleDto>> Handle(
        GetRoleByIdQuery request,
        CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .AsNoTracking()
            .Where(role => role.Id == request.Id)
            .Select(role => new RoleDto(
                role.Id,
                role.Code,
                role.Name,
                role.Description,
                role.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        return role is null
            ? Result<RoleDto>.Failure(Error.Validation("Role.NotFound", "Role was not found."))
            : Result<RoleDto>.Success(role);
    }
}
