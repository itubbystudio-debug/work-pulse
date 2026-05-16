namespace WorkPulse.Application.Features.MasterData.Commands.CreateEmployeeGroup;

public sealed record CreateEmployeeGroupResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
