using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Queries.GetPositionById;

public sealed class GetPositionByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetPositionByIdQuery, Result<PositionDetailsDto>>
{
    public async Task<Result<PositionDetailsDto>> Handle(
        GetPositionByIdQuery request,
        CancellationToken cancellationToken)
    {
        var position = await dbContext.Positions
            .AsNoTracking()
            .Where(item => item.Id == request.Id)
            .Select(item => new PositionDetailsDto(
                item.Id,
                item.DepartmentId,
                item.Department.Name,
                item.Name,
                item.Description,
                item.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        return position is null
            ? Result<PositionDetailsDto>.Failure(new Error("Position.NotFound", "Position was not found."))
            : Result<PositionDetailsDto>.Success(position);
    }
}
