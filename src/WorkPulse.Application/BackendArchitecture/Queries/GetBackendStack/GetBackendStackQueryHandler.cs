using MediatR;
using WorkPulse.Application.BackendArchitecture.Models;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Application.BackendArchitecture.Queries.GetBackendStack;

public sealed class GetBackendStackQueryHandler(
    ITechnologyCapabilityRepository repository,
    IRawSqlQueryCapability rawSqlQueryCapability) : IRequestHandler<GetBackendStackQuery, BackendStackResponseDto>
{
    public async Task<BackendStackResponseDto> Handle(GetBackendStackQuery request, CancellationToken cancellationToken)
    {
        var capabilities = await repository.ListAsync(cancellationToken);

        return new BackendStackResponseDto(
            Runtime: ".NET 10 Web API",
            Architecture: "Clean Architecture",
            CqrsLibrary: "MediatR",
            PrimaryDatabase: rawSqlQueryCapability.PrimaryDatabase,
            PrimaryOrm: rawSqlQueryCapability.PrimaryOrm,
            RawSqlTechnology: rawSqlQueryCapability.RawSqlTechnology,
            ReportingLibrary: "ClosedXML",
            RawSqlConfigured: rawSqlQueryCapability.IsConfigured,
            Capabilities: capabilities
                .Select(capability => new BackendStackItemDto(
                    capability.Category,
                    capability.Technology,
                    capability.Role))
                .ToArray());
    }
}
