namespace WorkPulse.Application.Features.Shifts.Commands.CreateShift;

public sealed record CreateShiftResponse(
    Guid Id,
    string Name,
    TimeOnly StartTime,
    TimeOnly EndTime);
