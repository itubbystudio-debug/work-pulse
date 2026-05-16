using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidays;

public sealed record GetCompanyHolidaysQuery(DateOnly? From = null, DateOnly? To = null)
    : IQuery<IReadOnlyCollection<CompanyHolidayListItemDto>>;
