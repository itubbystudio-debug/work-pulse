namespace WorkPulse.Application.Features.Roles.Commands.UpdateRole;

public sealed record UpdateRoleResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
