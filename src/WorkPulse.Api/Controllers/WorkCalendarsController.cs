using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.WorkCalendars.Commands.CreateWorkCalendar;
using WorkPulse.Application.Features.WorkCalendars.Queries.GetWorkCalendarById;

namespace WorkPulse.Api.Controllers;

public sealed class WorkCalendarsController(IMediator mediator) : BaseApiController(mediator)
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateWorkCalendarRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateWorkCalendarCommand(
            request.Name,
            request.StartDate,
            request.EndDate,
            request.ShiftCode,
            request.WorkRuleCode,
            request.ExceptionDates), cancellationToken);

        return FromResult(result);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetWorkCalendarByIdQuery(id), cancellationToken);
        return FromResult(result);
    }
}

public sealed record CreateWorkCalendarRequest(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    string? ShiftCode,
    string? WorkRuleCode,
    IReadOnlyCollection<WorkCalendarExceptionDateRequest> ExceptionDates);
