using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Diagnostics.Queries.GetArchitectureSummary;

public sealed record GetArchitectureSummaryQuery : IQuery<ArchitectureSummaryDto>;
