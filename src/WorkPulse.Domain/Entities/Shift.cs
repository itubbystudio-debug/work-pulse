using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class Shift : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private Shift()
    {
        Name = string.Empty;
    }

    public Shift(string name, TimeOnly startTime, TimeOnly endTime)
    {
        EnsureValid(name, startTime, endTime);

        Name = name.Trim();
        StartTime = startTime;
        EndTime = endTime;
    }

    public string Name { get; private set; }

    public TimeOnly StartTime { get; private set; }

    public TimeOnly EndTime { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public void Update(string name, TimeOnly startTime, TimeOnly endTime)
    {
        EnsureValid(name, startTime, endTime);

        Name = name.Trim();
        StartTime = startTime;
        EndTime = endTime;
    }

    private static void EnsureValid(string name, TimeOnly startTime, TimeOnly endTime)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("Shift.NameRequired", "Shift name is required.");
        }

        if (endTime <= startTime)
        {
            throw new BusinessRuleViolationException("Shift.InvalidTimeRange", "Shift end time must be after start time.");
        }
    }
}
