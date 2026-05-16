using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;

public sealed class UpdateCompanyHolidayCommandValidator : AbstractValidator<UpdateCompanyHolidayCommand>
{
    public UpdateCompanyHolidayCommandValidator(IApplicationDbContext dbContext)
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Date)
            .NotEmpty()
            .MustAsync(async (command, date, cancellationToken) =>
                !await dbContext.CompanyHolidays
                    .AsNoTracking()
                    .AnyAsync(holiday => holiday.Id != command.Id && holiday.Date == date, cancellationToken))
            .WithMessage("A company holiday already exists for this date.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(120);
    }
}
