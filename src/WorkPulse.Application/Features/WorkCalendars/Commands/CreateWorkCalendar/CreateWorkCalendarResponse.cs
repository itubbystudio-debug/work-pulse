namespace WorkPulse.Application.Features.WorkCalendars.Commands.CreateWorkCalendar;

public sealed record CreateWorkCalendarResponse(
    Guid Id,
    string Name,
    DateOnly StartDate,
    DateOnly EndDate);
