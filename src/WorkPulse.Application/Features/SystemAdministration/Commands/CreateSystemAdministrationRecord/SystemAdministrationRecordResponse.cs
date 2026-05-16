using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.CreateSystemAdministrationRecord;

public sealed record SystemAdministrationRecordResponse(
    Guid Id,
    SystemAdministrationRecordType Type,
    string Name,
    string? Code,
    string? Description,
    Guid? ParentId,
    bool IsActive);
