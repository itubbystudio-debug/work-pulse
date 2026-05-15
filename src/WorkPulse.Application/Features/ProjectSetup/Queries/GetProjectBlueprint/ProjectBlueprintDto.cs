namespace WorkPulse.Application.Features.ProjectSetup.Queries.GetProjectBlueprint;

public sealed record ProjectBlueprintDto(
    string SolutionName,
    string Architecture,
    IReadOnlyList<string> Layers,
    string Database,
    string PrimaryOrm,
    string SqlQueryTool,
    string Frontend,
    string Theme);
