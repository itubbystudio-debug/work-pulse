using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.GrantScreenActionPermission;

public sealed record GrantScreenActionPermissionResponse(
    Guid Id,
    Guid SubjectRecordId,
    Guid ScreenRecordId,
    ActionPermissionType Action,
    bool IsAllowed);
