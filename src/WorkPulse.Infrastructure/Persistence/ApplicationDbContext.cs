using Microsoft.EntityFrameworkCore;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence;

public sealed class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<TechnologyCapability> TechnologyCapabilities => Set<TechnologyCapability>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TechnologyCapability>(entity =>
        {
            entity.ToTable("TechnologyCapabilities");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Category).HasMaxLength(80).IsRequired();
            entity.Property(x => x.Technology).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Role).HasMaxLength(240).IsRequired();

            entity.HasData(
                new TechnologyCapability
                {
                    Id = 1,
                    Category = "API",
                    Technology = ".NET 10 Web API",
                    Role = "HTTP API delivery for backend modules"
                },
                new TechnologyCapability
                {
                    Id = 2,
                    Category = "Architecture",
                    Technology = "Clean Architecture",
                    Role = "Separation between API, application, domain, and infrastructure layers"
                },
                new TechnologyCapability
                {
                    Id = 3,
                    Category = "Application Pattern",
                    Technology = "MediatR CQRS",
                    Role = "Command and query dispatch through the application layer"
                },
                new TechnologyCapability
                {
                    Id = 4,
                    Category = "Data",
                    Technology = "EF Core + SQL Server",
                    Role = "Primary ORM and database access path"
                },
                new TechnologyCapability
                {
                    Id = 5,
                    Category = "Reporting",
                    Technology = "ClosedXML",
                    Role = "Excel report generation"
                });
        });
    }
}
