using FluentValidation;

namespace WorkPulse.Application.Features.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
