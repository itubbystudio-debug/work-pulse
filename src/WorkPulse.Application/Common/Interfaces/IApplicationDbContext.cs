using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Workspace> Workspaces { get; }

    DbSet<WorkCalendar> WorkCalendars { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
