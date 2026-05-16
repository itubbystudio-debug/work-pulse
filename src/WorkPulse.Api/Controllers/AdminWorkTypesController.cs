using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.WorkTypes.Commands.CreateWorkType;
using WorkPulse.Application.Features.WorkTypes.Commands.UpdateWorkType;
using WorkPulse.Application.Features.WorkTypes.Queries.GetWorkTypes;

namespace WorkPulse.Api.Controllers;

[AllowAnonymous]
[Route("api/v{version:int}/admin/work-types")]
public sealed class AdminWorkTypesController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetWorkTypes(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetWorkTypesQuery(), cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWorkType(
        WorkTypeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateWorkTypeCommand(
                request.Code,
                request.Name,
                request.Description,
                request.IsActive,
                request.PolicySettingsJson),
            cancellationToken);

        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateWorkType(
        Guid id,
        WorkTypeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateWorkTypeCommand(
                id,
                request.Code,
                request.Name,
                request.Description,
                request.IsActive,
                request.PolicySettingsJson),
            cancellationToken);

        return FromResult(result);
    }
}

public sealed record WorkTypeRequest(
    string Code,
    string Name,
    string? Description,
    bool IsActive = true,
    string? PolicySettingsJson = null);
