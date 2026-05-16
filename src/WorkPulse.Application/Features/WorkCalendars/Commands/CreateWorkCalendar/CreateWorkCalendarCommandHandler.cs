using MediatR;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.WorkCalendars.Commands.CreateWorkCalendar;

public sealed class CreateWorkCalendarCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateWorkCalendarCommand, Result<CreateWorkCalendarResponse>>
{
    public async Task<Result<CreateWorkCalendarResponse>> Handle(
        CreateWorkCalendarCommand request,
        CancellationToken cancellationToken)
    {
        var exceptionDates = request.ExceptionDates
            .Select(exceptionDate => new WorkCalendarExceptionDate(
                exceptionDate.Date,
                exceptionDate.IsWorkingDay,
                exceptionDate.Description));

        var calendar = new WorkCalendar(
            request.Name,
            request.StartDate,
            request.EndDate,
            request.ShiftCode,
            request.WorkRuleCode,
            exceptionDates);

        dbContext.WorkCalendars.Add(calendar);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateWorkCalendarResponse>.Success(new CreateWorkCalendarResponse(
            calendar.Id,
            calendar.Name,
            calendar.StartDate,
            calendar.EndDate));
    }
}
