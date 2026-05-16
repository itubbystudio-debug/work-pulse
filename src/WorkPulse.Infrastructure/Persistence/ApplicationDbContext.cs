using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence;

public sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Workspace> Workspaces => Set<Workspace>();

    public DbSet<SystemUser> SystemUsers => Set<SystemUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
