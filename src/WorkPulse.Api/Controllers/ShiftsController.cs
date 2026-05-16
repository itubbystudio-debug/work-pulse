using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.Shifts.Commands.CreateShift;
using WorkPulse.Application.Features.Shifts.Commands.UpdateShift;
using WorkPulse.Application.Features.Shifts.Queries.GetShiftById;

namespace WorkPulse.Api.Controllers;

[Authorize(Roles = "Admin")]
public sealed class ShiftsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateShiftRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateShiftCommand(request.Name, request.StartTime, request.EndTime),
            cancellationToken);

        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateShiftRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateShiftCommand(id, request.Name, request.StartTime, request.EndTime),
            cancellationToken);

        return FromResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetShiftByIdQuery(id), cancellationToken);
        return FromResult(result);
    }
}

public sealed record CreateShiftRequest(
    string Name,
    TimeOnly StartTime,
    TimeOnly EndTime);

public sealed record UpdateShiftRequest(
    string Name,
    TimeOnly StartTime,
    TimeOnly EndTime);
