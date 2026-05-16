using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;

public sealed record DeleteCompanyHolidayCommand(Guid Id) : ICommand;
