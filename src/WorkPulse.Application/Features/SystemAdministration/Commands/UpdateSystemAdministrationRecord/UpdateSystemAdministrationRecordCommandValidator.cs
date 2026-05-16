using FluentValidation;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.UpdateSystemAdministrationRecord;

public sealed class UpdateSystemAdministrationRecordCommandValidator
    : AbstractValidator<UpdateSystemAdministrationRecordCommand>
{
    public UpdateSystemAdministrationRecordCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(command => command.Code)
            .MaximumLength(80);

        RuleFor(command => command.Description)
            .MaximumLength(500);
    }
}
