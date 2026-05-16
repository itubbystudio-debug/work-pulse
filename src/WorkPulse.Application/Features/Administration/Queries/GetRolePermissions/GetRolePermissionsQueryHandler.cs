using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Administration.Queries.GetRolePermissions;

public sealed class GetRolePermissionsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetRolePermissionsQuery, Result<RolePermissionsDto>>
{
    public async Task<Result<RolePermissionsDto>> Handle(
        GetRolePermissionsQuery request,
        CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .AsNoTracking()
            .Where(role => role.Id == request.RoleId)
            .Select(role => new { role.Id, role.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (role is null)
        {
            return Result<RolePermissionsDto>.Failure(
                new Error("Role.NotFound", "Role was not found."));
        }

        var permissions = await dbContext.RolePermissions
            .AsNoTracking()
            .Where(rolePermission => rolePermission.RoleId == request.RoleId)
            .Join(
                dbContext.Permissions.AsNoTracking(),
                rolePermission => rolePermission.PermissionId,
                permission => permission.Id,
                (rolePermission, permission) => new PermissionAssignmentDto(
                    permission.Id,
                    permission.Code,
                    permission.Name))
            .OrderBy(permission => permission.Code)
            .ToArrayAsync(cancellationToken);

        return Result<RolePermissionsDto>.Success(new RolePermissionsDto(role.Id, role.Name, permissions));
    }
}
