using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Commands.DeleteDepartment;

public sealed class DeleteDepartmentCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteDepartmentCommand, Result>
{
    public async Task<Result> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (department is null)
        {
            return Result.Failure(new Error("Department.NotFound", "Department was not found."));
        }

        dbContext.Departments.Remove(department);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
