using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.WorkCalendars.Queries.GetWorkCalendarById;

public sealed record GetWorkCalendarByIdQuery(Guid Id) : IQuery<WorkCalendarDto>;
