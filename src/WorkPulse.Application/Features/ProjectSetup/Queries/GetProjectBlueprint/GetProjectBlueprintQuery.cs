using MediatR;

namespace WorkPulse.Application.Features.ProjectSetup.Queries.GetProjectBlueprint;

public sealed record GetProjectBlueprintQuery() : IRequest<ProjectBlueprintDto>;
