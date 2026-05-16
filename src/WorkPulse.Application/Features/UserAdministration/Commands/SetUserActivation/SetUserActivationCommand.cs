using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.UserAdministration.Commands.SetUserActivation;

public sealed record SetUserActivationCommand(Guid Id, bool IsActive) : ICommand<SetUserActivationResponse>;
