namespace WorkPulse.Application.Features.Shifts.Queries.GetShiftById;

public sealed record ShiftDto(
    Guid Id,
    string Name,
    TimeOnly StartTime,
    TimeOnly EndTime);
