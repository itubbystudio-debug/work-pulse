namespace WorkPulse.Application.Features.Administration.Queries.GetRolePermissions;

public sealed record RolePermissionsDto(
    Guid RoleId,
    string RoleName,
    IReadOnlyCollection<PermissionAssignmentDto> Permissions);

public sealed record PermissionAssignmentDto(Guid Id, string Code, string Name);
