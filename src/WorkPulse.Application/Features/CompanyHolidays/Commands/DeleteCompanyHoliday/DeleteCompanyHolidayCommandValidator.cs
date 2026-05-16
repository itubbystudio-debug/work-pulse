using FluentValidation;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;

public sealed class DeleteCompanyHolidayCommandValidator : AbstractValidator<DeleteCompanyHolidayCommand>
{
    public DeleteCompanyHolidayCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}
