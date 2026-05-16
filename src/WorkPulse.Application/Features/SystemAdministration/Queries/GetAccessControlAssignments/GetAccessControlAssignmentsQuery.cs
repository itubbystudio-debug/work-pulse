using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.SystemAdministration.Queries.GetAccessControlAssignments;

public sealed record GetAccessControlAssignmentsQuery(Guid? SubjectRecordId)
    : IQuery<IReadOnlyCollection<AccessControlAssignmentDto>>;
