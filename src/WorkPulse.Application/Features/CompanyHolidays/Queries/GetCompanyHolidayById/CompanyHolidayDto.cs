namespace WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidayById;

public sealed record CompanyHolidayDto(Guid Id, DateOnly Date, string Name);
