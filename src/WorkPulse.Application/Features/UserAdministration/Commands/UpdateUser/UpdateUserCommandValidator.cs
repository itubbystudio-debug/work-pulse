using FluentValidation;

namespace WorkPulse.Application.Features.UserAdministration.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

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
