using MediatR;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Diagnostics.Queries.GetArchitectureSummary;

public sealed class GetArchitectureSummaryQueryHandler
    : IRequestHandler<GetArchitectureSummaryQuery, Result<ArchitectureSummaryDto>>
{
    public Task<Result<ArchitectureSummaryDto>> Handle(
        GetArchitectureSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var summary = new ArchitectureSummaryDto(
            ".NET 10 Web API",
            "Clean Architecture with CQRS via MediatR",
            "SQL Server",
            "EF Core",
            "Dapper",
            "ClosedXML");

        return Task.FromResult(Result<ArchitectureSummaryDto>.Success(summary));
    }
}
