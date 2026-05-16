namespace WorkPulse.Application.Features.MasterData.Commands.UpdatePosition;

public sealed record UpdatePositionResponse(
    Guid Id,
    Guid DepartmentId,
    string DepartmentName,
    string Name,
    string? Description,
    bool IsActive);
