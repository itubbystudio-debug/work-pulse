using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.SystemAdministration.Commands.CreateSystemAdministrationRecord;
using WorkPulse.Application.Features.SystemAdministration.Commands.GrantScreenActionPermission;
using WorkPulse.Application.Features.SystemAdministration.Commands.UpdateSystemAdministrationRecord;
using WorkPulse.Application.Features.SystemAdministration.Queries.GetAccessControlAssignments;
using WorkPulse.Application.Features.SystemAdministration.Queries.ListSystemAdministrationRecords;
using WorkPulse.Domain.Enums;

namespace WorkPulse.Api.Controllers;

[Authorize(Roles = "Administrator")]
public sealed class SystemAdministrationController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet("records")]
    public async Task<IActionResult> ListRecords(
        [FromQuery] SystemAdministrationRecordType? type,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListSystemAdministrationRecordsQuery(type, isActive), cancellationToken);
        return FromResult(result);
    }

    [HttpPost("records")]
    public async Task<IActionResult> CreateRecord(
        CreateSystemAdministrationRecordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateSystemAdministrationRecordCommand(
            request.Type,
            request.Name,
            request.Code,
            request.Description,
            request.ParentId,
            request.IsActive), cancellationToken);

        return FromResult(result);
    }

    [HttpPut("records/{id:guid}")]
    public async Task<IActionResult> UpdateRecord(
        Guid id,
        UpdateSystemAdministrationRecordRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateSystemAdministrationRecordCommand(
            id,
            request.Name,
            request.Code,
            request.Description,
            request.ParentId,
            request.IsActive), cancellationToken);

        return FromResult(result);
    }

    [HttpGet("access-control")]
    public async Task<IActionResult> GetAccessControl(
        [FromQuery] Guid? subjectRecordId,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetAccessControlAssignmentsQuery(subjectRecordId), cancellationToken);
        return FromResult(result);
    }

    [HttpPost("access-control")]
    public async Task<IActionResult> GrantScreenActionPermission(
        GrantScreenActionPermissionRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GrantScreenActionPermissionCommand(
            request.SubjectRecordId,
            request.ScreenRecordId,
            request.Action,
            request.IsAllowed), cancellationToken);

        return FromResult(result);
    }
}

public sealed record CreateSystemAdministrationRecordRequest(
    SystemAdministrationRecordType Type,
    string Name,
    string? Code,
    string? Description,
    Guid? ParentId,
    bool IsActive = true);

public sealed record UpdateSystemAdministrationRecordRequest(
    string Name,
    string? Code,
    string? Description,
    Guid? ParentId,
    bool IsActive = true);

public sealed record GrantScreenActionPermissionRequest(
    Guid SubjectRecordId,
    Guid ScreenRecordId,
    ActionPermissionType Action,
    bool IsAllowed = true);
