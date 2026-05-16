namespace WorkPulse.Application.Features.Roles.Queries.GetRoles;

public sealed record RoleListItemDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive);
