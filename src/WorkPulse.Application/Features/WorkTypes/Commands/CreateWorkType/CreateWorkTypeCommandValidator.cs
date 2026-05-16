using FluentValidation;

namespace WorkPulse.Application.Features.WorkTypes.Commands.CreateWorkType;

public sealed class CreateWorkTypeCommandValidator : AbstractValidator<CreateWorkTypeCommand>
{
    public CreateWorkTypeCommandValidator()
    {
        RuleFor(command => command.Code)
            .NotEmpty()
            .MaximumLength(40)
            .Matches("^[A-Za-z0-9_-]+$");

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.Description)
            .MaximumLength(500);

        RuleFor(command => command.PolicySettingsJson)
            .MaximumLength(4000);
    }
}
