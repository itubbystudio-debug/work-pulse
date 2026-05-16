using FluentValidation;

namespace WorkPulse.Application.Features.UserAdministration.Commands.SetUserActivation;

public sealed class SetUserActivationCommandValidator : AbstractValidator<SetUserActivationCommand>
{
    public SetUserActivationCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
