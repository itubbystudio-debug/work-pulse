namespace WorkPulse.Application.Features.WorkTypes.Queries.GetWorkTypes;

public sealed record WorkTypeDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive,
    string? PolicySettingsJson);
