namespace WorkPulse.Application.Features.MasterData.Commands.CreateDepartment;

public sealed record CreateDepartmentResponse(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
