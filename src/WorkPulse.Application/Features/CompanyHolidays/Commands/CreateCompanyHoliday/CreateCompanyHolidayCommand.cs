using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.CreateCompanyHoliday;

public sealed record CreateCompanyHolidayCommand(DateOnly Date, string Name) : ICommand<CreateCompanyHolidayResponse>;
