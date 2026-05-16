namespace WorkPulse.Application.Features.MasterData.Commands.UpdateDepartment;

public sealed record UpdateDepartmentResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
