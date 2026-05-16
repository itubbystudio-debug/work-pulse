namespace WorkPulse.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;

public sealed record UpdateCompanyHolidayResponse(Guid Id, DateOnly Date, string Name);
