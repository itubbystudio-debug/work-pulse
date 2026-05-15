using Microsoft.EntityFrameworkCore;

namespace WorkPulse.Infrastructure.Persistence;

public sealed class ApplicationDbContextInitializer(ApplicationDbContext dbContext)
{
    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        return dbContext.Database.MigrateAsync(cancellationToken);
    }
}
