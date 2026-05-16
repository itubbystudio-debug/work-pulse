using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;

public sealed class UpdateCompanyHolidayCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateCompanyHolidayCommand, Result<UpdateCompanyHolidayResponse>>
{
    public async Task<Result<UpdateCompanyHolidayResponse>> Handle(
        UpdateCompanyHolidayCommand request,
        CancellationToken cancellationToken)
    {
        var holiday = await dbContext.CompanyHolidays
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (holiday is null)
        {
            return Result<UpdateCompanyHolidayResponse>.Failure(
                new Error("CompanyHoliday.NotFound", "Company holiday was not found."));
        }

        var duplicateExists = await dbContext.CompanyHolidays
            .AsNoTracking()
            .AnyAsync(item => item.Id != request.Id && item.Date == request.Date, cancellationToken);

        if (duplicateExists)
        {
            return Result<UpdateCompanyHolidayResponse>.Failure(
                new Error("CompanyHoliday.DuplicateDate", "A company holiday already exists for this date."));
        }

        holiday.Update(request.Date, request.Name);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateCompanyHolidayResponse>.Success(
            new UpdateCompanyHolidayResponse(holiday.Id, holiday.Date, holiday.Name));
    }
}
