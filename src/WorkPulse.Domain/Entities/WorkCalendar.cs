using WorkPulse.Domain.Common;

namespace WorkPulse.Domain.Entities;

public sealed class WorkCalendar : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private readonly List<WorkCalendarExceptionDate> _exceptionDates = [];

    private WorkCalendar()
    {
        Name = string.Empty;
    }

    public WorkCalendar(
        string name,
        DateOnly startDate,
        DateOnly endDate,
        string? shiftCode,
        string? workRuleCode,
        IEnumerable<WorkCalendarExceptionDate> exceptionDates)
    {
        ValidateDateRange(startDate, endDate);

        Name = string.IsNullOrWhiteSpace(name)
            ? throw new BusinessRuleViolationException("WorkCalendar.NameRequired", "Work calendar name is required.")
            : name.Trim();
        StartDate = startDate;
        EndDate = endDate;
        ShiftCode = string.IsNullOrWhiteSpace(shiftCode) ? null : shiftCode.Trim();
        WorkRuleCode = string.IsNullOrWhiteSpace(workRuleCode) ? null : workRuleCode.Trim();

        SetExceptionDates(exceptionDates);
    }

    public string Name { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public string? ShiftCode { get; private set; }

    public string? WorkRuleCode { get; private set; }

    public IReadOnlyCollection<WorkCalendarExceptionDate> ExceptionDates => _exceptionDates.AsReadOnly();

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    private void SetExceptionDates(IEnumerable<WorkCalendarExceptionDate> exceptionDates)
    {
        var dates = exceptionDates.ToArray();
        var duplicateDate = dates
            .GroupBy(exceptionDate => exceptionDate.Date)
            .FirstOrDefault(group => group.Count() > 1);

        if (duplicateDate is not null)
        {
            throw new BusinessRuleViolationException(
                "WorkCalendar.DuplicateExceptionDate",
                "Work calendar exception dates must be unique.");
        }

        if (dates.Any(exceptionDate => exceptionDate.Date < StartDate || exceptionDate.Date > EndDate))
        {
            throw new BusinessRuleViolationException(
                "WorkCalendar.ExceptionDateOutOfRange",
                "Work calendar exception dates must be inside the calendar date range.");
        }

        _exceptionDates.Clear();
        _exceptionDates.AddRange(dates);
    }

    private static void ValidateDateRange(DateOnly startDate, DateOnly endDate)
    {
        if (endDate < startDate)
        {
            throw new BusinessRuleViolationException(
                "WorkCalendar.InvalidDateRange",
                "Work calendar end date must be on or after start date.");
        }
    }
}
