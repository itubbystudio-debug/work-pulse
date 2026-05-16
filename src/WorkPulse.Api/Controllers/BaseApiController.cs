using MediatR;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Api.Controllers;

[ApiController]
[Route("api/v{version:int}/[controller]")]
public abstract class BaseApiController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator { get; } = mediator;

    protected IActionResult FromResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok(new { success = true });
        }

        return BadRequest(ToProblemDetails(result));
    }

    protected IActionResult FromResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(new { success = true, data = result.Value });
        }

        return BadRequest(ToProblemDetails(result));
    }

    private static ProblemDetails ToProblemDetails(Result result)
    {
        return new ProblemDetails
        {
            Title = "Request failed.",
            Status = StatusCodes.Status400BadRequest,
            Extensions =
            {
                ["errors"] = result.Errors
                    .GroupBy(error => error.Code)
                    .ToDictionary(group => group.Key, group => group.Select(error => error.Message).ToArray()),
            },
        };
    }
}
