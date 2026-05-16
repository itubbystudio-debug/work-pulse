using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.WorkCalendars.Queries.GetWorkCalendarById;

public sealed class GetWorkCalendarByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetWorkCalendarByIdQuery, Result<WorkCalendarDto>>
{
    public async Task<Result<WorkCalendarDto>> Handle(
        GetWorkCalendarByIdQuery request,
        CancellationToken cancellationToken)
    {
        var calendar = await dbContext.WorkCalendars
            .AsNoTracking()
            .Where(item => item.Id == request.Id)
            .Select(item => new WorkCalendarDto(
                item.Id,
                item.Name,
                item.StartDate,
                item.EndDate,
                item.ShiftCode,
                item.WorkRuleCode,
                item.ExceptionDates
                    .OrderBy(exceptionDate => exceptionDate.Date)
                    .Select(exceptionDate => new WorkCalendarExceptionDateDto(
                        exceptionDate.Date,
                        exceptionDate.IsWorkingDay,
                        exceptionDate.Description))
                    .ToArray()))
            .FirstOrDefaultAsync(cancellationToken);

        return calendar is null
            ? Result<WorkCalendarDto>.Failure(new Error("WorkCalendar.NotFound", "Work calendar was not found."))
            : Result<WorkCalendarDto>.Success(calendar);
    }
}
