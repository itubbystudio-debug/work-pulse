using MediatR;

namespace WorkPulse.Application.Features.ProjectSetup.Commands.InitializeWorkspace;

public sealed record InitializeWorkspaceCommand(string? ProjectName) : IRequest<WorkspaceProfileDto>;
