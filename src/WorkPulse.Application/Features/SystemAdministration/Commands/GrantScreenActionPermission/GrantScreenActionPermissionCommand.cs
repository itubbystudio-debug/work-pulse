using WorkPulse.Application.Common.Messaging;
using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.GrantScreenActionPermission;

public sealed record GrantScreenActionPermissionCommand(
    Guid SubjectRecordId,
    Guid ScreenRecordId,
    ActionPermissionType Action,
    bool IsAllowed) : ICommand<GrantScreenActionPermissionResponse>;
