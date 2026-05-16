using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Shifts.Commands.UpdateShift;

public sealed class UpdateShiftCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateShiftCommand, Result<UpdateShiftResponse>>
{
    public async Task<Result<UpdateShiftResponse>> Handle(
        UpdateShiftCommand request,
        CancellationToken cancellationToken)
    {
        var shift = await dbContext.Shifts
            .FirstOrDefaultAsync(existingShift => existingShift.Id == request.Id, cancellationToken);

        if (shift is null)
        {
            return Result<UpdateShiftResponse>.Failure(
                new Error("Shift.NotFound", "Shift was not found."));
        }

        var hasOverlap = await dbContext.Shifts
            .AsNoTracking()
            .AnyAsync(
                existingShift =>
                    existingShift.Id != request.Id
                    && existingShift.StartTime < request.EndTime
                    && existingShift.EndTime > request.StartTime,
                cancellationToken);

        if (hasOverlap)
        {
            return Result<UpdateShiftResponse>.Failure(
                new Error("Shift.TimeOverlap", "Shift time range overlaps with an existing shift."));
        }

        shift.Update(request.Name, request.StartTime, request.EndTime);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateShiftResponse>.Success(
            new UpdateShiftResponse(shift.Id, shift.Name, shift.StartTime, shift.EndTime));
    }
}
