using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Queries.ListSystemAdministrationRecords;

public sealed record SystemAdministrationRecordDto(
    Guid Id,
    SystemAdministrationRecordType Type,
    string Name,
    string? Code,
    string? Description,
    Guid? ParentId,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
