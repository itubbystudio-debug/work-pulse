namespace WorkPulse.Application.Features.ProjectSetup.Commands.InitializeWorkspace;

public sealed record WorkspaceProfileDto(Guid Id, string Name, DateTime CreatedAtUtc);
