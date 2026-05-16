using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.MasterData.Commands.CreateDepartment;
using WorkPulse.Application.Features.MasterData.Commands.CreateEmployeeGroup;
using WorkPulse.Application.Features.MasterData.Commands.CreatePosition;
using WorkPulse.Application.Features.MasterData.Commands.DeleteDepartment;
using WorkPulse.Application.Features.MasterData.Commands.DeleteEmployeeGroup;
using WorkPulse.Application.Features.MasterData.Commands.DeletePosition;
using WorkPulse.Application.Features.MasterData.Commands.UpdateDepartment;
using WorkPulse.Application.Features.MasterData.Commands.UpdateEmployeeGroup;
using WorkPulse.Application.Features.MasterData.Commands.UpdatePosition;
using WorkPulse.Application.Features.MasterData.Queries.GetDepartmentById;
using WorkPulse.Application.Features.MasterData.Queries.GetDepartments;
using WorkPulse.Application.Features.MasterData.Queries.GetEmployeeGroupById;
using WorkPulse.Application.Features.MasterData.Queries.GetEmployeeGroups;
using WorkPulse.Application.Features.MasterData.Queries.GetPositionById;
using WorkPulse.Application.Features.MasterData.Queries.GetPositions;

namespace WorkPulse.Api.Controllers;

[AllowAnonymous]
public sealed class AdminMasterDataController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("departments")]
    public async Task<IActionResult> GetDepartments(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDepartmentsQuery(), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("departments/{id:guid}")]
    public async Task<IActionResult> GetDepartment(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetDepartmentByIdQuery(id), cancellationToken);
        return FromResult(result);
    }

    [HttpPost("departments")]
    public async Task<IActionResult> CreateDepartment(
        DepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateDepartmentCommand(request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpPut("departments/{id:guid}")]
    public async Task<IActionResult> UpdateDepartment(
        Guid id,
        DepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateDepartmentCommand(id, request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpDelete("departments/{id:guid}")]
    public async Task<IActionResult> DeleteDepartment(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteDepartmentCommand(id), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions(
        [FromQuery] Guid? departmentId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPositionsQuery(departmentId), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("positions/{id:guid}")]
    public async Task<IActionResult> GetPosition(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPositionByIdQuery(id), cancellationToken);
        return FromResult(result);
    }

    [HttpPost("positions")]
    public async Task<IActionResult> CreatePosition(
        PositionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreatePositionCommand(request.DepartmentId, request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpPut("positions/{id:guid}")]
    public async Task<IActionResult> UpdatePosition(
        Guid id,
        PositionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdatePositionCommand(id, request.DepartmentId, request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpDelete("positions/{id:guid}")]
    public async Task<IActionResult> DeletePosition(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeletePositionCommand(id), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("employee-groups")]
    public async Task<IActionResult> GetEmployeeGroups(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetEmployeeGroupsQuery(), cancellationToken);
        return FromResult(result);
    }

    [HttpGet("employee-groups/{id:guid}")]
    public async Task<IActionResult> GetEmployeeGroup(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetEmployeeGroupByIdQuery(id), cancellationToken);
        return FromResult(result);
    }

    [HttpPost("employee-groups")]
    public async Task<IActionResult> CreateEmployeeGroup(
        EmployeeGroupRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new CreateEmployeeGroupCommand(request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpPut("employee-groups/{id:guid}")]
    public async Task<IActionResult> UpdateEmployeeGroup(
        Guid id,
        EmployeeGroupRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(
            new UpdateEmployeeGroupCommand(id, request.Name, request.Description, request.IsActive),
            cancellationToken);

        return FromResult(result);
    }

    [HttpDelete("employee-groups/{id:guid}")]
    public async Task<IActionResult> DeleteEmployeeGroup(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new DeleteEmployeeGroupCommand(id), cancellationToken);
        return FromResult(result);
    }
}

public sealed record DepartmentRequest(string Name, string? Description, bool IsActive = true);

public sealed record PositionRequest(Guid DepartmentId, string Name, string? Description, bool IsActive = true);

public sealed record EmployeeGroupRequest(string Name, string? Description, bool IsActive = true);
