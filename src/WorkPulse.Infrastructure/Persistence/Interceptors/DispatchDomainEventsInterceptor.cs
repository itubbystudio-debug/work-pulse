using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorkPulse.Application.Common.Events;
using WorkPulse.Domain.Common;

namespace WorkPulse.Infrastructure.Persistence.Interceptors;

public sealed class DispatchDomainEventsInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null)
        {
            return;
        }

        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(entry => entry.Entity.DomainEvents.Count > 0)
            .Select(entry => entry.Entity)
            .ToArray();

        var domainEvents = entities
            .SelectMany(entity => entity.DomainEvents)
            .ToArray();

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(notificationType, domainEvent)!;
            await publisher.Publish(notification, cancellationToken);
        }
    }
}
