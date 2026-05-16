using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateDepartmentCommand, Result<UpdateDepartmentResponse>>
{
    public async Task<Result<UpdateDepartmentResponse>> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (department is null)
        {
            return Result<UpdateDepartmentResponse>.Failure(new Error(
                "Department.NotFound",
                "Department was not found."));
        }

        department.Update(request.Name, request.Description, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateDepartmentResponse>.Success(new UpdateDepartmentResponse(
            department.Id,
            department.Name,
            department.Description,
            department.IsActive));
    }
}
