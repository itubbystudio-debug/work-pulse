using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.Administration.Commands.AssignRolePermissions;
using WorkPulse.Application.Features.Administration.Queries.GetRolePermissions;

namespace WorkPulse.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/v{version:int}/admin/roles/{roleId:guid}/permissions")]
public sealed class AdminRolePermissionsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpPut]
    public async Task<IActionResult> Assign(
        Guid roleId,
        AssignRolePermissionsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new AssignRolePermissionsCommand(roleId, request.PermissionIds),
            cancellationToken);

        return FromResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> Get(Guid roleId, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetRolePermissionsQuery(roleId), cancellationToken);
        return FromResult(result);
    }
}

public sealed record AssignRolePermissionsRequest(IReadOnlyCollection<Guid> PermissionIds);
