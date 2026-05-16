using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.SystemAdministration.Queries.ListSystemAdministrationRecords;

public sealed class ListSystemAdministrationRecordsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<ListSystemAdministrationRecordsQuery, Result<IReadOnlyCollection<SystemAdministrationRecordDto>>>
{
    public async Task<Result<IReadOnlyCollection<SystemAdministrationRecordDto>>> Handle(
        ListSystemAdministrationRecordsQuery request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.SystemAdministrationRecords.AsNoTracking();

        if (request.Type.HasValue)
        {
            query = query.Where(record => record.Type == request.Type.Value);
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(record => record.IsActive == request.IsActive.Value);
        }

        var records = await query
            .OrderBy(record => record.Type)
            .ThenBy(record => record.Name)
            .Select(record => new SystemAdministrationRecordDto(
                record.Id,
                record.Type,
                record.Name,
                record.Code,
                record.Description,
                record.ParentId,
                record.IsActive,
                record.CreatedAt,
                record.ModifiedAt))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<SystemAdministrationRecordDto>>.Success(records);
    }
}
