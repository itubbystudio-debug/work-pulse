using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Api.Contracts;
using WorkPulse.Application.BackendArchitecture.Commands.ExportBackendStackReport;
using WorkPulse.Application.BackendArchitecture.Models;
using WorkPulse.Application.BackendArchitecture.Queries.GetBackendStack;

namespace WorkPulse.Api.Controllers;

[ApiController]
[Route("api/architecture")]
public sealed class ArchitectureController(ISender sender) : ControllerBase
{
    [HttpGet("stack")]
    [ProducesResponseType(typeof(ApiResponse<BackendStackResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<BackendStackResponseDto>>> GetStack(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new GetBackendStackQuery(), cancellationToken);
        return Ok(new ApiResponse<BackendStackResponseDto>(true, response));
    }

    [HttpPost("report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ExportReport(
        [FromBody] ExportBackendStackReportRequest request,
        CancellationToken cancellationToken)
    {
        var report = await sender.Send(
            new ExportBackendStackReportCommand(request.ReportTitle),
            cancellationToken);

        return File(report.Content, report.ContentType, report.FileName);
    }
}
