using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext dbContext, CancellationToken cancellationToken = default)
    {
        if (dbContext.TechnologyCapabilities.Any())
        {
            return;
        }

        dbContext.TechnologyCapabilities.AddRange(
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

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
