using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Commands.UpdatePosition;

public sealed class UpdatePositionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdatePositionCommand, Result<UpdatePositionResponse>>
{
    public async Task<Result<UpdatePositionResponse>> Handle(
        UpdatePositionCommand request,
        CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(item => item.Id == request.DepartmentId, cancellationToken);

        if (department is null)
        {
            return Result<UpdatePositionResponse>.Failure(new Error(
                "Department.NotFound",
                "Department was not found."));
        }

        var position = await dbContext.Positions
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (position is null)
        {
            return Result<UpdatePositionResponse>.Failure(new Error(
                "Position.NotFound",
                "Position was not found."));
        }

        position.Update(request.DepartmentId, request.Name, request.Description, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdatePositionResponse>.Success(new UpdatePositionResponse(
            position.Id,
            position.DepartmentId,
            department.Name,
            position.Name,
            position.Description,
            position.IsActive));
    }
}
