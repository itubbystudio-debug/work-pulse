using FluentValidation;

namespace WorkPulse.Application.Features.MasterData.Commands.DeleteEmployeeGroup;

public sealed class DeleteEmployeeGroupCommandValidator : AbstractValidator<DeleteEmployeeGroupCommand>
{
    public DeleteEmployeeGroupCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
