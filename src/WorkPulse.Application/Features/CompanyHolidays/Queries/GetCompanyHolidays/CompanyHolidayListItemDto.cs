namespace WorkPulse.Application.Features.CompanyHolidays.Queries.GetCompanyHolidays;

public sealed record CompanyHolidayListItemDto(Guid Id, DateOnly Date, string Name);
