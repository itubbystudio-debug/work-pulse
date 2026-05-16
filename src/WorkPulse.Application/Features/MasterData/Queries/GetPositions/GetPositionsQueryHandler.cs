using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Queries.GetPositions;

public sealed class GetPositionsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetPositionsQuery, Result<IReadOnlyCollection<PositionDto>>>
{
    public async Task<Result<IReadOnlyCollection<PositionDto>>> Handle(
        GetPositionsQuery request,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Positions.AsNoTracking();

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(position => position.DepartmentId == request.DepartmentId.Value);
        }

        var positions = await query
            .OrderBy(position => position.Department.Name)
            .ThenBy(position => position.Name)
            .Select(position => new PositionDto(
                position.Id,
                position.DepartmentId,
                position.Department.Name,
                position.Name,
                position.Description,
                position.IsActive))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<PositionDto>>.Success(positions);
    }
}
