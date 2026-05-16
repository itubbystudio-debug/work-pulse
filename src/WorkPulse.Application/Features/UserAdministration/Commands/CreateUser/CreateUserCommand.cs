using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.UserAdministration.Commands.CreateUser;

public sealed record CreateUserCommand(
    string IdentityUserId,
    string UserName,
    string Email,
    string DisplayName,
    string Role,
    bool IsActive) : ICommand<CreateUserResponse>;
