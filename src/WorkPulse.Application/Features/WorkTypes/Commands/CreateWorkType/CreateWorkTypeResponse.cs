namespace WorkPulse.Application.Features.WorkTypes.Commands.CreateWorkType;

public sealed record CreateWorkTypeResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive,
    string? PolicySettingsJson);
