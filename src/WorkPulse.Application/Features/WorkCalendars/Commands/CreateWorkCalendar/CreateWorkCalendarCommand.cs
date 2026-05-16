using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.WorkCalendars.Commands.CreateWorkCalendar;

public sealed record CreateWorkCalendarCommand(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    string? ShiftCode,
    string? WorkRuleCode,
    IReadOnlyCollection<WorkCalendarExceptionDateRequest> ExceptionDates) : ICommand<CreateWorkCalendarResponse>;

public sealed record WorkCalendarExceptionDateRequest(
    DateOnly Date,
    bool IsWorkingDay,
    string? Description);
