using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Workspace> Workspaces { get; }

    DbSet<SystemUser> SystemUsers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
