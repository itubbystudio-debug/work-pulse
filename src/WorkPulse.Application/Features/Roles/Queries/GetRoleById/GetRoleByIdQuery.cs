using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Roles.Queries.GetRoleById;

public sealed record GetRoleByIdQuery(Guid Id) : IQuery<RoleDto>;
