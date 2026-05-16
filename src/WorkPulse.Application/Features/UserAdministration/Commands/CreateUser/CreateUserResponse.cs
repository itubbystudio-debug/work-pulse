namespace WorkPulse.Application.Features.UserAdministration.Commands.CreateUser;

public sealed record CreateUserResponse(
    Guid Id,
    string IdentityUserId,
    string UserName,
    string Email,
    string DisplayName,
    string Role,
    bool IsActive);
