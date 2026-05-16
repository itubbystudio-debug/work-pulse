namespace WorkPulse.Application.Features.MasterData.Commands.UpdateEmployeeGroup;

public sealed record UpdateEmployeeGroupResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
