using MediatR;
using WorkPulse.Application.Abstractions.Persistence;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.ProjectSetup.Commands.InitializeWorkspace;

public sealed class InitializeWorkspaceCommandHandler(
    IWorkspaceProfileRepository workspaceProfileRepository,
    TimeProvider timeProvider) : IRequestHandler<InitializeWorkspaceCommand, WorkspaceProfileDto>
{
    public async Task<WorkspaceProfileDto> Handle(InitializeWorkspaceCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ProjectName))
        {
            throw new ArgumentException("Project name is required.", nameof(request.ProjectName));
        }

        var workspaceProfile = WorkspaceProfile.Create(
            request.ProjectName,
            timeProvider.GetUtcNow().UtcDateTime);

        await workspaceProfileRepository.AddAsync(workspaceProfile, cancellationToken);

        return new WorkspaceProfileDto(
            workspaceProfile.Id,
            workspaceProfile.Name,
            workspaceProfile.CreatedAtUtc);
    }
}
