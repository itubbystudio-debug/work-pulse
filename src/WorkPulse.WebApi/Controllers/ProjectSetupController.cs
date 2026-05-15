using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.ProjectSetup.Commands.InitializeWorkspace;
using WorkPulse.Application.Features.ProjectSetup.Queries.GetProjectBlueprint;

namespace WorkPulse.WebApi.Controllers;

[ApiController]
[Route("api/project-setup")]
public sealed class ProjectSetupController(IMediator mediator) : ControllerBase
{
    [HttpGet("blueprint")]
    [ProducesResponseType<ProjectBlueprintDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectBlueprintDto>> GetBlueprint(CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new GetProjectBlueprintQuery(), cancellationToken);
        return Ok(response);
    }

    [HttpPost("workspace")]
    [ProducesResponseType<WorkspaceProfileDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<WorkspaceProfileDto>> InitializeWorkspace(
        [FromBody] InitializeWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new InitializeWorkspaceCommand(request.ProjectName), cancellationToken);
        return Ok(response);
    }
}

public sealed class InitializeWorkspaceRequest
{
    [Required]
    [MinLength(1)]
    public string? ProjectName { get; init; }
}
