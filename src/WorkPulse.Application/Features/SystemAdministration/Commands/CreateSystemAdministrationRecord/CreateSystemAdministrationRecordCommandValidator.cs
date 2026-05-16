using FluentValidation;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.CreateSystemAdministrationRecord;

public sealed class CreateSystemAdministrationRecordCommandValidator
    : AbstractValidator<CreateSystemAdministrationRecordCommand>
{
    public CreateSystemAdministrationRecordCommandValidator()
    {
        RuleFor(command => command.Type)
            .IsInEnum();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Code)
            .MaximumLength(80);

        RuleFor(command => command.Description)
            .MaximumLength(500);
    }
}
