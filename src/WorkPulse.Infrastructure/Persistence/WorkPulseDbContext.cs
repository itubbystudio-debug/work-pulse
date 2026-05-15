using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence;

public class WorkPulseDbContext(DbContextOptions<WorkPulseDbContext> options) : DbContext(options)
{
    public DbSet<WorkspaceProfile> WorkspaceProfiles => Set<WorkspaceProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkspaceProfile>(entity =>
        {
            entity.ToTable("WorkspaceProfiles");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.CreatedAtUtc).IsRequired();
        });
    }
}
