using FluentValidation;

namespace WorkPulse.Application.Features.Shifts.Commands.UpdateShift;

public sealed class UpdateShiftCommandValidator : AbstractValidator<UpdateShiftCommand>
{
    public UpdateShiftCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.EndTime)
            .GreaterThan(command => command.StartTime)
            .WithMessage("Shift end time must be after start time.");
    }
}
