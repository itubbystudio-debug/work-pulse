namespace WorkPulse.Application.Features.UserAdministration.Commands.SetUserActivation;

public sealed record SetUserActivationResponse(Guid Id, bool IsActive);
