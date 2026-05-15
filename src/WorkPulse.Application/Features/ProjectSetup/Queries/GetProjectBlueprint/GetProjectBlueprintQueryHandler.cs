using MediatR;

namespace WorkPulse.Application.Features.ProjectSetup.Queries.GetProjectBlueprint;

public sealed class GetProjectBlueprintQueryHandler : IRequestHandler<GetProjectBlueprintQuery, ProjectBlueprintDto>
{
    public Task<ProjectBlueprintDto> Handle(GetProjectBlueprintQuery request, CancellationToken cancellationToken)
    {
        var response = new ProjectBlueprintDto(
            "WorkPulse",
            "Clean Standard + CQRS + MediatR",
            [
                "WorkPulse.Domain",
                "WorkPulse.Application",
                "WorkPulse.Infrastructure",
                "WorkPulse.WebApi"
            ],
            "SQL Server",
            "Entity Framework Core",
            "Dapper",
            "Vue 3 + PrimeVue",
            "Sakai-inspired PrimeVue shell");

        return Task.FromResult(response);
    }
}
