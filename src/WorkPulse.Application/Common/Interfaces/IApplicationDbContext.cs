using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Workspace> Workspaces { get; }

    DbSet<Department> Departments { get; }

    DbSet<Position> Positions { get; }

    DbSet<EmployeeGroup> EmployeeGroups { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
