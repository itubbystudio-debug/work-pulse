using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Shifts.Queries.GetShiftById;

public sealed class GetShiftByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetShiftByIdQuery, Result<ShiftDto>>
{
    public async Task<Result<ShiftDto>> Handle(
        GetShiftByIdQuery request,
        CancellationToken cancellationToken)
    {
        var shift = await dbContext.Shifts
            .AsNoTracking()
            .Where(existingShift => existingShift.Id == request.Id)
            .Select(existingShift => new ShiftDto(
                existingShift.Id,
                existingShift.Name,
                existingShift.StartTime,
                existingShift.EndTime))
            .FirstOrDefaultAsync(cancellationToken);

        return shift is null
            ? Result<ShiftDto>.Failure(new Error("Shift.NotFound", "Shift was not found."))
            : Result<ShiftDto>.Success(shift);
    }
}
