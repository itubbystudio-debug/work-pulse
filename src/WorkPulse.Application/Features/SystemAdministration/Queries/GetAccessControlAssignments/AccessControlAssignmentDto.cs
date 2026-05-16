using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Queries.GetAccessControlAssignments;

public sealed record AccessControlAssignmentDto(
    Guid Id,
    Guid SubjectRecordId,
    string SubjectName,
    Guid ScreenRecordId,
    string ScreenName,
    ActionPermissionType Action,
    bool IsAllowed);
