using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;

public sealed class CreateCompanyHolidayCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateCompanyHolidayCommand, Result<CreateCompanyHolidayResponse>>
{
    public async Task<Result<CreateCompanyHolidayResponse>> Handle(
        CreateCompanyHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.CompanyHolidays
            .AsNoTracking()
            .AnyAsync(holiday => holiday.Date == request.Date, cancellationToken);

        if (exists)
        {
            return Result<CreateCompanyHolidayResponse>.Failure(
                new Error("CompanyHoliday.DuplicateDate", "A company holiday already exists for this date."));
        }

        var holiday = new CompanyHoliday(request.Date, request.Name);

        dbContext.CompanyHolidays.Add(holiday);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateCompanyHolidayResponse>.Success(
            new CreateCompanyHolidayResponse(holiday.Id, holiday.Date, holiday.Name));
    }
}
