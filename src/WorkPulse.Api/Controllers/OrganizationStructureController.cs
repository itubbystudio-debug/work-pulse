using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.OrganizationStructure.Commands.CreateOrganizationNode;
using WorkPulse.Application.Features.OrganizationStructure.Commands.UpdateOrganizationNode;
using WorkPulse.Application.Features.OrganizationStructure.Queries.GetOrganizationNodes;

namespace WorkPulse.Api.Controllers;

public sealed class OrganizationStructureController(IMediator mediator) : BaseApiController(mediator)
{
    [AllowAnonymous]
    [HttpGet("nodes")]
    public async Task<IActionResult> GetNodes(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetOrganizationNodesQuery(), cancellationToken);
        return FromResult(result);
    }

    [AllowAnonymous]
    [HttpPost("nodes")]
    public async Task<IActionResult> CreateNode(
        CreateOrganizationNodeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateOrganizationNodeCommand(request.Name, request.ParentId),
            cancellationToken);

        return FromResult(result);
    }

    [AllowAnonymous]
    [HttpPut("nodes/{id:guid}")]
    public async Task<IActionResult> UpdateNode(
        Guid id,
        UpdateOrganizationNodeRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateOrganizationNodeCommand(id, request.Name, request.ParentId),
            cancellationToken);

        return FromResult(result);
    }
}

public sealed record CreateOrganizationNodeRequest(string Name, Guid? ParentId);

public sealed record UpdateOrganizationNodeRequest(string Name, Guid? ParentId);
