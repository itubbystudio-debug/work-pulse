using FluentValidation;

namespace WorkPulse.Application.Features.WorkCalendars.Commands.CreateWorkCalendar;

public sealed class CreateWorkCalendarCommandValidator : AbstractValidator<CreateWorkCalendarCommand>
{
    public CreateWorkCalendarCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);

        RuleFor(command => command.EndDate)
            .GreaterThanOrEqualTo(command => command.StartDate)
            .WithMessage("End date must be on or after start date.");

        RuleFor(command => command.ShiftCode)
            .MaximumLength(50);

        RuleFor(command => command.WorkRuleCode)
            .MaximumLength(50);

        RuleFor(command => command.ExceptionDates)
            .NotNull()
            .Must(HaveUniqueDates)
            .WithMessage("Exception dates must be unique.");

        RuleForEach(command => command.ExceptionDates)
            .ChildRules(exceptionDate =>
            {
                exceptionDate.RuleFor(item => item.Description)
                    .MaximumLength(200);
            });

        RuleForEach(command => command.ExceptionDates)
            .Must((command, exceptionDate) =>
                exceptionDate.Date >= command.StartDate && exceptionDate.Date <= command.EndDate)
            .WithMessage("Exception dates must be inside the calendar date range.");
    }

    private static bool HaveUniqueDates(IReadOnlyCollection<WorkCalendarExceptionDateRequest>? exceptionDates)
    {
        return exceptionDates is null
            || exceptionDates.Select(exceptionDate => exceptionDate.Date).Distinct().Count() == exceptionDates.Count;
    }
}
