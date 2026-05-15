using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Abstractions.Persistence;

public interface IWorkspaceProfileRepository
{
    Task AddAsync(WorkspaceProfile workspaceProfile, CancellationToken cancellationToken);
}
