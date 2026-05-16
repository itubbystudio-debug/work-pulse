namespace WorkPulse.Application.Features.Administration.Commands.AssignRolePermissions;

public sealed record AssignRolePermissionsResponse(Guid RoleId, IReadOnlyCollection<Guid> PermissionIds);
