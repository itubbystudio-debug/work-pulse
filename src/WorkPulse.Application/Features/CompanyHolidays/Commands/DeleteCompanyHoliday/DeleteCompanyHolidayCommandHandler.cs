using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.CompanyHolidays.Commands.DeleteCompanyHoliday;

public sealed class DeleteCompanyHolidayCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteCompanyHolidayCommand, Result>
{
    public async Task<Result> Handle(DeleteCompanyHolidayCommand request, CancellationToken cancellationToken)
    {
        var holiday = await dbContext.CompanyHolidays
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (holiday is null)
        {
            return Result.Failure(new Error("CompanyHoliday.NotFound", "Company holiday was not found."));
        }

        dbContext.CompanyHolidays.Remove(holiday);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
