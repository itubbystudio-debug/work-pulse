using FluentValidation;

namespace WorkPulse.Application.Features.MasterData.Commands.CreateEmployeeGroup;

public sealed class CreateEmployeeGroupCommandValidator : AbstractValidator<CreateEmployeeGroupCommand>
{
    public CreateEmployeeGroupCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.Description)
            .MaximumLength(500);
    }
}
