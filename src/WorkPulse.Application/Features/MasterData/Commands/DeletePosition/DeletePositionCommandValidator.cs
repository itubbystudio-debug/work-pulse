using FluentValidation;

namespace WorkPulse.Application.Features.MasterData.Commands.DeletePosition;

public sealed class DeletePositionCommandValidator : AbstractValidator<DeletePositionCommand>
{
    public DeletePositionCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
