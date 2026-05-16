using FluentValidation;

namespace WorkPulse.Application.Features.UserAdministration.Commands.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.IdentityUserId)
            .NotEmpty()
            .MaximumLength(450);

        RuleFor(command => command.UserName)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(command => command.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256);

        RuleFor(command => command.DisplayName)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Role)
            .NotEmpty()
            .MaximumLength(100);
    }
}
