using WorkPulse.Application.Common.Messaging;
using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Queries.ListSystemAdministrationRecords;

public sealed record ListSystemAdministrationRecordsQuery(
    SystemAdministrationRecordType? Type,
    bool? IsActive) : IQuery<IReadOnlyCollection<SystemAdministrationRecordDto>>;
