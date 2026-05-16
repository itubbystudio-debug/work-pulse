using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.Administration.Commands.AssignRolePermissions;

public sealed class AssignRolePermissionsCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<AssignRolePermissionsCommand, Result<AssignRolePermissionsResponse>>
{
    public async Task<Result<AssignRolePermissionsResponse>> Handle(
        AssignRolePermissionsCommand request,
        CancellationToken cancellationToken)
    {
        var roleExists = await dbContext.Roles
            .AsNoTracking()
            .AnyAsync(role => role.Id == request.RoleId, cancellationToken);

        if (!roleExists)
        {
            return Result<AssignRolePermissionsResponse>.Failure(
                new Error("Role.NotFound", "Role was not found."));
        }

        var requestedPermissionIds = request.PermissionIds
            .Distinct()
            .ToArray();

        var existingPermissionIds = await dbContext.Permissions
            .AsNoTracking()
            .Where(permission => requestedPermissionIds.Contains(permission.Id))
            .Select(permission => permission.Id)
            .ToArrayAsync(cancellationToken);

        var invalidPermissionIds = requestedPermissionIds
            .Except(existingPermissionIds)
            .ToArray();

        if (invalidPermissionIds.Length > 0)
        {
            return Result<AssignRolePermissionsResponse>.Failure(
                new Error(
                    "Permission.InvalidReference",
                    $"Invalid permission IDs: {string.Join(", ", invalidPermissionIds)}"));
        }

        var currentMappings = await dbContext.RolePermissions
            .Where(rolePermission => rolePermission.RoleId == request.RoleId)
            .ToArrayAsync(cancellationToken);

        dbContext.RolePermissions.RemoveRange(currentMappings);

        foreach (var permissionId in requestedPermissionIds)
        {
            dbContext.RolePermissions.Add(new RolePermission(request.RoleId, permissionId));
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<AssignRolePermissionsResponse>.Success(
            new AssignRolePermissionsResponse(request.RoleId, requestedPermissionIds));
    }
}
