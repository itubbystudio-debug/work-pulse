using FluentValidation;

namespace WorkPulse.Application.Features.MasterData.Commands.UpdateEmployeeGroup;

public sealed class UpdateEmployeeGroupCommandValidator : AbstractValidator<UpdateEmployeeGroupCommand>
{
    public UpdateEmployeeGroupCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.Description)
            .MaximumLength(500);
    }
}
