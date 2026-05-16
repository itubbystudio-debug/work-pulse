using FluentValidation;

namespace WorkPulse.Application.Features.MasterData.Commands.DeleteDepartment;

public sealed class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
