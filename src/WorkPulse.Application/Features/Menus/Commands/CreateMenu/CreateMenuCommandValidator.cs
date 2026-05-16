using FluentValidation;

namespace WorkPulse.Application.Features.Menus.Commands.CreateMenu;

public sealed class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.Route)
            .MaximumLength(256);

        RuleFor(command => command.Icon)
            .MaximumLength(80);

        RuleFor(command => command.DisplayOrder)
            .GreaterThanOrEqualTo(0);
    }
}
