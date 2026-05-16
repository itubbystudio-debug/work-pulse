using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Administration.Commands.AssignRolePermissions;

public sealed record AssignRolePermissionsCommand(
    Guid RoleId,
    IReadOnlyCollection<Guid> PermissionIds) : ICommand<AssignRolePermissionsResponse>;
