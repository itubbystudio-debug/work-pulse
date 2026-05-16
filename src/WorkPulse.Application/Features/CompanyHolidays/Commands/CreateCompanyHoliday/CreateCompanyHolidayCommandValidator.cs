using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;

public sealed class CreateCompanyHolidayCommandValidator : AbstractValidator<CreateCompanyHolidayCommand>
{
    public CreateCompanyHolidayCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(command => command.Date)
            .NotEmpty()
            .MustAsync(async (date, cancellationToken) =>
                !await dbContext.CompanyHolidays
                    .AsNoTracking()
                    .AnyAsync(holiday => holiday.Date == date, cancellationToken))
            .WithMessage("A company holiday already exists for this date.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);
    }
}
