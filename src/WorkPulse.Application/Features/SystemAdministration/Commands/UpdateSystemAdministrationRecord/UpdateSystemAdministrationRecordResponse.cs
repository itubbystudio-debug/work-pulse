using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.UpdateSystemAdministrationRecord;

public sealed record UpdateSystemAdministrationRecordResponse(
    Guid Id,
    SystemAdministrationRecordType Type,
    string Name,
    string? Code,
    string? Description,
    Guid? ParentId,
    bool IsActive);
