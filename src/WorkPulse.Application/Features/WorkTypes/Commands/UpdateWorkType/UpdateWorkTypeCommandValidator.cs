using FluentValidation;

namespace WorkPulse.Application.Features.WorkTypes.Commands.UpdateWorkType;

public sealed class UpdateWorkTypeCommandValidator : AbstractValidator<UpdateWorkTypeCommand>
{
    public UpdateWorkTypeCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

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
