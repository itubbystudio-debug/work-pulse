using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Diagnostics.Commands.CreateWorkspace;

public sealed record CreateWorkspaceCommand(string Name) : ICommand<CreateWorkspaceResponse>;
