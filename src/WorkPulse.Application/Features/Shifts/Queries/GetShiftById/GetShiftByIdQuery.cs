using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Shifts.Queries.GetShiftById;

public sealed record GetShiftByIdQuery(Guid Id) : IQuery<ShiftDto>;
