using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Shifts.Commands.CreateShift;

public sealed record CreateShiftCommand(
    string Name,
    TimeOnly StartTime,
    TimeOnly EndTime) : ICommand<CreateShiftResponse>;
