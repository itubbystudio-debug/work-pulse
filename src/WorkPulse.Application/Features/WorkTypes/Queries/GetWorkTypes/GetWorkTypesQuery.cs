using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.WorkTypes.Queries.GetWorkTypes;

public sealed record GetWorkTypesQuery : IQuery<IReadOnlyCollection<WorkTypeDto>>;
