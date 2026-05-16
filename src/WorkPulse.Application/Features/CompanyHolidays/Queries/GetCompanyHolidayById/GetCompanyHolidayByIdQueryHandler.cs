using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidayById;

public sealed class GetCompanyHolidayByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCompanyHolidayByIdQuery, Result<CompanyHolidayDto>>
{
    public async Task<Result<CompanyHolidayDto>> Handle(
        GetCompanyHolidayByIdQuery request,
        CancellationToken cancellationToken)
    {
        var holiday = await dbContext.CompanyHolidays
            .AsNoTracking()
            .Where(item => item.Id == request.Id)
            .Select(item => new CompanyHolidayDto(item.Id, item.Date, item.Name))
            .FirstOrDefaultAsync(cancellationToken);

        return holiday is null
            ? Result<CompanyHolidayDto>.Failure(new Error("CompanyHoliday.NotFound", "Company holiday was not found."))
            : Result<CompanyHolidayDto>.Success(holiday);
    }
}
