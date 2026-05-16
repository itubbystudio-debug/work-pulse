using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.UserAdministration.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid Id,
    string UserName,
    string Email,
    string DisplayName,
    string Role) : ICommand<UpdateUserResponse>;
