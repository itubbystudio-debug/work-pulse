using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Workspace> Workspaces { get; }

    DbSet<WorkType> WorkTypes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
