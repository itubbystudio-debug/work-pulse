using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.Roles.Commands.CreateRole;
using WorkPulse.Application.Features.Roles.Commands.DeleteRole;
using WorkPulse.Application.Features.Roles.Commands.UpdateRole;
using WorkPulse.Application.Features.Roles.Queries.GetRoleById;
using WorkPulse.Application.Features.Roles.Queries.GetRoles;

namespace WorkPulse.Api.Controllers;

[AllowAnonymous]
public sealed class RolesController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetRolesQuery(), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetRoleById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetRoleByIdQuery(id), cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRole(
        CreateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateRoleCommand(request.Code, request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateRole(
        Guid id,
        UpdateRoleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateRoleCommand(id, request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteRole(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteRoleCommand(id), cancellationToken);
        return FromResult(result);
    }
}

public sealed record CreateRoleRequest(
    string Code,
    string Name,
    string? Description,
    bool IsActive = true);

public sealed record UpdateRoleRequest(
    string Name,
    string? Description,
    bool IsActive);
