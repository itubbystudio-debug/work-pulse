namespace WorkPulse.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;

public sealed record CreateCompanyHolidayResponse(Guid Id, DateOnly Date, string Name);
