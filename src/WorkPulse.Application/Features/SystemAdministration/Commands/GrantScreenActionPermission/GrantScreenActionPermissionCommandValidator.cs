using FluentValidation;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.GrantScreenActionPermission;

public sealed class GrantScreenActionPermissionCommandValidator
    : AbstractValidator<GrantScreenActionPermissionCommand>
{
    public GrantScreenActionPermissionCommandValidator()
    {
        RuleFor(command => command.SubjectRecordId)
            .NotEmpty();

        RuleFor(command => command.ScreenRecordId)
            .NotEmpty();

        RuleFor(command => command.Action)
            .IsInEnum();
    }
}
