using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidays;

public sealed class GetCompanyHolidaysQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCompanyHolidaysQuery, Result<IReadOnlyCollection<CompanyHolidayListItemDto>>>
{
    public async Task<Result<IReadOnlyCollection<CompanyHolidayListItemDto>>> Handle(
        GetCompanyHolidaysQuery request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.CompanyHolidays.AsNoTracking();

        if (request.From.HasValue)
        {
            query = query.Where(item => item.Date >= request.From.Value);
        }

        if (request.To.HasValue)
        {
            query = query.Where(item => item.Date <= request.To.Value);
        }

        var holidays = await query
            .OrderBy(item => item.Date)
            .ThenBy(item => item.Name)
            .Select(item => new CompanyHolidayListItemDto(item.Id, item.Date, item.Name))
            .ToArrayAsync(cancellationToken);

        return Result<IReadOnlyCollection<CompanyHolidayListItemDto>>.Success(holidays);
    }
}
