using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.SystemAdministration.Queries.GetAccessControlAssignments;

public sealed class GetAccessControlAssignmentsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetAccessControlAssignmentsQuery, Result<IReadOnlyCollection<AccessControlAssignmentDto>>>
{
    public async Task<Result<IReadOnlyCollection<AccessControlAssignmentDto>>> Handle(
        GetAccessControlAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        var query =
            from assignment in dbContext.AccessControlAssignments.AsNoTracking()
            join subject in dbContext.SystemAdministrationRecords.AsNoTracking()
                on assignment.SubjectRecordId equals subject.Id
            join screen in dbContext.SystemAdministrationRecords.AsNoTracking()
                on assignment.ScreenRecordId equals screen.Id
            select new { assignment, subject, screen };

        if (request.SubjectRecordId.HasValue)
        {
            query = query.Where(item => item.assignment.SubjectRecordId == request.SubjectRecordId.Value);
        }

        var assignments = await query
            .OrderBy(item => item.subject.Name)
            .ThenBy(item => item.screen.Name)
            .ThenBy(item => item.assignment.Action)
            .Select(item => new AccessControlAssignmentDto(
                item.assignment.Id,
                item.assignment.SubjectRecordId,
                item.subject.Name,
                item.assignment.ScreenRecordId,
                item.screen.Name,
                item.assignment.Action,
                item.assignment.IsAllowed))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<AccessControlAssignmentDto>>.Success(assignments);
    }
}
