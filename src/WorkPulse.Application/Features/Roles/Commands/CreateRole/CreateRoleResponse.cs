namespace WorkPulse.Application.Features.Roles.Commands.CreateRole;

public sealed record CreateRoleResponse(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
