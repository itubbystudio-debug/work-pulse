using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Commands.UpdateEmployeeGroup;

public sealed class UpdateEmployeeGroupCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateEmployeeGroupCommand, Result<UpdateEmployeeGroupResponse>>
{
    public async Task<Result<UpdateEmployeeGroupResponse>> Handle(
        UpdateEmployeeGroupCommand request,
        CancellationToken cancellationToken)
    {
        var employeeGroup = await dbContext.EmployeeGroups
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (employeeGroup is null)
        {
            return Result<UpdateEmployeeGroupResponse>.Failure(new Error(
                "EmployeeGroup.NotFound",
                "Employee group was not found."));
        }

        employeeGroup.Update(request.Name, request.Description, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateEmployeeGroupResponse>.Success(new UpdateEmployeeGroupResponse(
            employeeGroup.Id,
            employeeGroup.Name,
            employeeGroup.Description,
            employeeGroup.IsActive));
    }
}
