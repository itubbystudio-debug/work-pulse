using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Workspace> Workspaces { get; }

    DbSet<SystemAdministrationRecord> SystemAdministrationRecords { get; }

    DbSet<AccessControlAssignment> AccessControlAssignments { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
