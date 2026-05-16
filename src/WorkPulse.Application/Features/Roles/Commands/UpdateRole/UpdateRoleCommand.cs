using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Roles.Commands.UpdateRole;

public sealed record UpdateRoleCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive) : ICommand<UpdateRoleResponse>;
