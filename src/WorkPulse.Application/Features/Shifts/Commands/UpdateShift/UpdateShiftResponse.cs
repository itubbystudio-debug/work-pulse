namespace WorkPulse.Application.Features.Shifts.Commands.UpdateShift;

public sealed record UpdateShiftResponse(
    Guid Id,
    string Name,
    TimeOnly StartTime,
    TimeOnly EndTime);
