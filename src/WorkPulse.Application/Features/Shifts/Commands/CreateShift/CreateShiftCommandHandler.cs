using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.Shifts.Commands.CreateShift;

public sealed class CreateShiftCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateShiftCommand, Result<CreateShiftResponse>>
{
    public async Task<Result<CreateShiftResponse>> Handle(
        CreateShiftCommand request,
        CancellationToken cancellationToken)
    {
        var hasOverlap = await dbContext.Shifts
            .AsNoTracking()
            .AnyAsync(
                shift => shift.StartTime < request.EndTime && shift.EndTime > request.StartTime,
                cancellationToken);

        if (hasOverlap)
        {
            return Result<CreateShiftResponse>.Failure(
                new Error("Shift.TimeOverlap", "Shift time range overlaps with an existing shift."));
        }

        var shift = new Shift(request.Name, request.StartTime, request.EndTime);

        dbContext.Shifts.Add(shift);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateShiftResponse>.Success(
            new CreateShiftResponse(shift.Id, shift.Name, shift.StartTime, shift.EndTime));
    }
}
