using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;
using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.GrantScreenActionPermission;

public sealed class GrantScreenActionPermissionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GrantScreenActionPermissionCommand, Result<GrantScreenActionPermissionResponse>>
{
    private static readonly SystemAdministrationRecordType[] SubjectTypes =
    [
        SystemAdministrationRecordType.User,
        SystemAdministrationRecordType.Role,
    ];

    private static readonly SystemAdministrationRecordType[] ScreenTypes =
    [
        SystemAdministrationRecordType.Menu,
        SystemAdministrationRecordType.SubMenu,
        SystemAdministrationRecordType.ScreenAccess,
    ];

    public async Task<Result<GrantScreenActionPermissionResponse>> Handle(
        GrantScreenActionPermissionCommand request,
        CancellationToken cancellationToken)
    {
        var subject = await dbContext.SystemAdministrationRecords
            .AsNoTracking()
            .Where(record => record.Id == request.SubjectRecordId)
            .Select(record => new { record.Type, record.IsActive })
            .FirstOrDefaultAsync(cancellationToken);

        if (subject is null || !SubjectTypes.Contains(subject.Type) || !subject.IsActive)
        {
            return Result<GrantScreenActionPermissionResponse>.Failure(
                new Error("SystemAdministration.InvalidPermissionSubject", "Permission subject must be an active user or role record."));
        }

        var screen = await dbContext.SystemAdministrationRecords
            .AsNoTracking()
            .Where(record => record.Id == request.ScreenRecordId)
            .Select(record => new { record.Type, record.IsActive })
            .FirstOrDefaultAsync(cancellationToken);

        if (screen is null || !ScreenTypes.Contains(screen.Type) || !screen.IsActive)
        {
            return Result<GrantScreenActionPermissionResponse>.Failure(
                new Error("SystemAdministration.InvalidPermissionScreen", "Permission screen must be an active menu, sub-menu, or screen access record."));
        }

        var assignment = await dbContext.AccessControlAssignments
            .FirstOrDefaultAsync(
                item => item.SubjectRecordId == request.SubjectRecordId
                    && item.ScreenRecordId == request.ScreenRecordId
                    && item.Action == request.Action,
                cancellationToken);

        if (assignment is null)
        {
            assignment = AccessControlAssignment.Create(
                request.SubjectRecordId,
                request.ScreenRecordId,
                request.Action,
                request.IsAllowed);

            dbContext.AccessControlAssignments.Add(assignment);
        }
        else
        {
            assignment.SetAllowed(request.IsAllowed);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<GrantScreenActionPermissionResponse>.Success(new GrantScreenActionPermissionResponse(
            assignment.Id,
            assignment.SubjectRecordId,
            assignment.ScreenRecordId,
            assignment.Action,
            assignment.IsAllowed));
    }
}
