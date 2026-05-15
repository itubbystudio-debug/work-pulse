using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Domain.Common;

namespace WorkPulse.Infrastructure.Persistence.Interceptors;

public sealed class AuditableEntityInterceptor(
    ICurrentUserService currentUserService,
    IDateTimeProvider dateTimeProvider)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateAuditableEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateAuditableEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = dateTimeProvider.UtcNow;
                entry.Entity.CreatedBy = currentUserService.UserId;
            }

            if (entry.State is EntityState.Modified or EntityState.Added)
            {
                entry.Entity.ModifiedAt = dateTimeProvider.UtcNow;
                entry.Entity.ModifiedBy = currentUserService.UserId;
            }
        }
    }
}
