using FluentValidation;

namespace WorkPulse.Application.Features.Shifts.Commands.CreateShift;

public sealed class CreateShiftCommandValidator : AbstractValidator<CreateShiftCommand>
{
    public CreateShiftCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.EndTime)
            .GreaterThan(command => command.StartTime)
            .WithMessage("Shift end time must be after start time.");
    }
}
