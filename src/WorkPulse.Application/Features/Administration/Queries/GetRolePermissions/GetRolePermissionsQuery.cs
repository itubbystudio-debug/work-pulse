using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Administration.Queries.GetRolePermissions;

public sealed record GetRolePermissionsQuery(Guid RoleId) : IQuery<RolePermissionsDto>;
