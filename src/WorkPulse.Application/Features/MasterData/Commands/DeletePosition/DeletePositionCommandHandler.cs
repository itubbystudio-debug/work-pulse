using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Commands.DeletePosition;

public sealed class DeletePositionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeletePositionCommand, Result>
{
    public async Task<Result> Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        var position = await dbContext.Positions
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (position is null)
        {
            return Result.Failure(new Error("Position.NotFound", "Position was not found."));
        }

        dbContext.Positions.Remove(position);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
