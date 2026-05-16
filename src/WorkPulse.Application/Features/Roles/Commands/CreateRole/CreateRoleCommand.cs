using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Roles.Commands.CreateRole;

public sealed record CreateRoleCommand(
    string Code,
    string Name,
    string? Description,
    bool IsActive = true) : ICommand<CreateRoleResponse>;
