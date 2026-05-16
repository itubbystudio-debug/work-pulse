using FluentValidation;

namespace WorkPulse.Application.Features.Menus.Commands.UpdateMenu;

public sealed class UpdateMenuCommandValidator : AbstractValidator<UpdateMenuCommand>
{
    public UpdateMenuCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

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
