using FluentValidation;

namespace WorkPulse.Application.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(command => command.Code)
            .NotEmpty()
            .MaximumLength(80)
            .Matches("^[A-Za-z][A-Za-z0-9_-]*$")
            .WithMessage("Role code must start with a letter and contain only letters, numbers, hyphens, or underscores.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.Description)
            .MaximumLength(500);
    }
}
