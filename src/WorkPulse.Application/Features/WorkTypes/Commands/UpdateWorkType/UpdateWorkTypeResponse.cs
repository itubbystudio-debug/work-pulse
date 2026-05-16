namespace WorkPulse.Application.Features.WorkTypes.Commands.UpdateWorkType;

public sealed record UpdateWorkTypeResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive,
    string? PolicySettingsJson);
