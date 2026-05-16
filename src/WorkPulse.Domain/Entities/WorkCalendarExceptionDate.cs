using WorkPulse.Domain.Common;

namespace WorkPulse.Domain.Entities;

public sealed class WorkCalendarExceptionDate : BaseEntity
{
    private WorkCalendarExceptionDate()
    {
    }

    public WorkCalendarExceptionDate(DateOnly date, bool isWorkingDay, string? description)
    {
        Date = date;
        IsWorkingDay = isWorkingDay;
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }

    public Guid WorkCalendarId { get; private set; }

    public DateOnly Date { get; private set; }

    public bool IsWorkingDay { get; private set; }

    public string? Description { get; private set; }
}
