using MediatR;
using WorkPulse.Domain.Common;

namespace WorkPulse.Application.Common.Events;

public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent) : INotification
    where TDomainEvent : DomainEvent;
