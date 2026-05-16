using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidayById;

public sealed record GetCompanyHolidayByIdQuery(Guid Id) : IQuery<CompanyHolidayDto>;
