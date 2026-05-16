using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Shifts.Commands.UpdateShift;

public sealed record UpdateShiftCommand(
    Guid Id,
    string Name,
    TimeOnly StartTime,
    TimeOnly EndTime) : ICommand<UpdateShiftResponse>;
