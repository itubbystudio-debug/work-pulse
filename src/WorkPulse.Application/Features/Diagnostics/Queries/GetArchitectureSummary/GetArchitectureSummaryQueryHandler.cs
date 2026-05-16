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
            "ClosedXML",
            new FrontendStackPolicyDto(
                "Angular",
                "PrimeNG",
                "Sakai Template",
                "All new frontend work in scope must use Angular as the primary frontend framework.",
                "Angular, PrimeNG, and Sakai Template versions are TBD until the project version policy is approved.",
                ["Form", "Table", "Button", "Card", "Modal"],
                "Non-approved UI libraries must be flagged for review and require explicit approval before use.",
                "Mixed legacy frontend modules require a confirmed migration or exception approach before implementation."));

        return Task.FromResult(Result<ArchitectureSummaryDto>.Success(summary));
    }
}
