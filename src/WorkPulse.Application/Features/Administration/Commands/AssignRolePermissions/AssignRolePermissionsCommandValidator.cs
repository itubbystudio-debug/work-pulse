using FluentValidation;

namespace WorkPulse.Application.Features.Administration.Commands.AssignRolePermissions;

public sealed class AssignRolePermissionsCommandValidator : AbstractValidator<AssignRolePermissionsCommand>
{
    public AssignRolePermissionsCommandValidator()
    {
        RuleFor(command => command.RoleId)
            .NotEmpty();

        RuleFor(command => command.PermissionIds)
            .NotNull();

        RuleForEach(command => command.PermissionIds)
            .NotEmpty();
    }
}
