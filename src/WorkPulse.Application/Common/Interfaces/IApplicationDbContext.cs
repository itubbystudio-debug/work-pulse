using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<CompanyProfile> CompanyProfiles { get; }

    DbSet<Workspace> Workspaces { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
