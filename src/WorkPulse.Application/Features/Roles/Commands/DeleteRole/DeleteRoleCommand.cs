using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Roles.Commands.DeleteRole;

public sealed record DeleteRoleCommand(Guid Id) : ICommand;
