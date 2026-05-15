using WorkPulse.Application.Abstractions.Persistence;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence;

public sealed class WorkspaceProfileRepository(WorkPulseDbContext dbContext) : IWorkspaceProfileRepository
{
    public async Task AddAsync(WorkspaceProfile workspaceProfile, CancellationToken cancellationToken)
    {
        await dbContext.WorkspaceProfiles.AddAsync(workspaceProfile, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
