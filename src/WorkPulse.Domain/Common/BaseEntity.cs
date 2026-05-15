namespace WorkPulse.Domain.Common;

public abstract class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = [];

    public Guid Id { get; protected set; } = Guid.NewGuid();

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
