namespace WorkPulse.Application.Features.WorkCalendars.Queries.GetWorkCalendarById;

public sealed record WorkCalendarDto(
    Guid Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    string? ShiftCode,
    string? WorkRuleCode,
    IReadOnlyCollection<WorkCalendarExceptionDateDto> ExceptionDates);

public sealed record WorkCalendarExceptionDateDto(
    DateOnly Date,
    bool IsWorkingDay,
    string? Description);
