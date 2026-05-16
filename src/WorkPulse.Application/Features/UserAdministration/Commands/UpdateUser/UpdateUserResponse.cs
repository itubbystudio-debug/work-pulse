namespace WorkPulse.Application.Features.UserAdministration.Commands.UpdateUser;

public sealed record UpdateUserResponse(
    Guid Id,
    string IdentityUserId,
    string UserName,
    string Email,
    string DisplayName,
    string Role,
    bool IsActive);
