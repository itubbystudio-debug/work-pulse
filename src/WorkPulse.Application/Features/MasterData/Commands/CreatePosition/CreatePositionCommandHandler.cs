using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.MasterData.Commands.CreatePosition;

public sealed class CreatePositionCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreatePositionCommand, Result<CreatePositionResponse>>
{
    public async Task<Result<CreatePositionResponse>> Handle(
        CreatePositionCommand request,
        CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(item => item.Id == request.DepartmentId, cancellationToken);

        if (department is null)
        {
            return Result<CreatePositionResponse>.Failure(new Error(
                "Department.NotFound",
                "Department was not found."));
        }

        var position = new Position(request.DepartmentId, request.Name, request.Description, request.IsActive);

        dbContext.Positions.Add(position);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreatePositionResponse>.Success(new CreatePositionResponse(
            position.Id,
            position.DepartmentId,
            department.Name,
            position.Name,
            position.Description,
            position.IsActive));
    }
}
