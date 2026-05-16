using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.WorkTypes.Queries.GetWorkTypes;

public sealed class GetWorkTypesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetWorkTypesQuery, Result<IReadOnlyCollection<WorkTypeDto>>>
{
    public async Task<Result<IReadOnlyCollection<WorkTypeDto>>> Handle(
        GetWorkTypesQuery request,
        CancellationToken cancellationToken)
    {
        var workTypes = await dbContext.WorkTypes
            .AsNoTracking()
            .OrderBy(workType => workType.Name)
            .Select(workType => new WorkTypeDto(
                workType.Id,
                workType.Code,
                workType.Name,
                workType.Description,
                workType.IsActive,
                workType.PolicySettingsJson))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<WorkTypeDto>>.Success(workTypes);
    }
}
