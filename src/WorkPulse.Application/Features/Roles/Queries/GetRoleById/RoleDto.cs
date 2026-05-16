namespace WorkPulse.Application.Features.Roles.Queries.GetRoleById;

public sealed record RoleDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
