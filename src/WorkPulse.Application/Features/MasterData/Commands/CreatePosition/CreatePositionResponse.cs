namespace WorkPulse.Application.Features.MasterData.Commands.CreatePosition;

public sealed record CreatePositionResponse(
    Guid Id,
    Guid DepartmentId,
    string DepartmentName,
    string Name,
    string? Description,
    bool IsActive);
