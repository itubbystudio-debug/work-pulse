using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.Diagnostics.Commands.CreateWorkspace;
using WorkPulse.Application.Features.Diagnostics.Queries.GetArchitectureSummary;
using WorkPulse.Application.Features.Diagnostics.Reports.ExportWorkspaces;

namespace WorkPulse.Api.Controllers;

public sealed class DiagnosticsController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("architecture")]
    public async Task<IActionResult> GetArchitectureSummary(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetArchitectureSummaryQuery(), cancellationToken);
        return FromResult(result);
    }

    [HttpPost("workspaces")]
    public async Task<IActionResult> CreateWorkspace(
        CreateWorkspaceRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateWorkspaceCommand(request.Name), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("workspaces/report")]
    public async Task<IActionResult> ExportWorkspaces(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ExportWorkspacesReportQuery(), cancellationToken);

        if (result.IsFailure || result.Value is null)
        {
            return FromResult(result);
        }

        return File(result.Value.Content, result.Value.ContentType, result.Value.FileName);
    }
}

public sealed record CreateWorkspaceRequest(string Name);
