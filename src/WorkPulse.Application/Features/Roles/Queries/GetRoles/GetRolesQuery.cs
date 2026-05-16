using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Roles.Queries.GetRoles;

public sealed record GetRolesQuery : IQuery<IReadOnlyCollection<RoleListItemDto>>;
