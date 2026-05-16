using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.UpdateCompanyHoliday;

public sealed record UpdateCompanyHolidayCommand(Guid Id, DateOnly Date, string Name) : ICommand<UpdateCompanyHolidayResponse>;
