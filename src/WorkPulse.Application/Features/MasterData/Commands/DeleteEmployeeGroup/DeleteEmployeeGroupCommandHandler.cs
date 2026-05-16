using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Commands.DeleteEmployeeGroup;

public sealed class DeleteEmployeeGroupCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteEmployeeGroupCommand, Result>
{
    public async Task<Result> Handle(DeleteEmployeeGroupCommand request, CancellationToken cancellationToken)
    {
        var employeeGroup = await dbContext.EmployeeGroups
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (employeeGroup is null)
        {
            return Result.Failure(new Error("EmployeeGroup.NotFound", "Employee group was not found."));
        }

        dbContext.EmployeeGroups.Remove(employeeGroup);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
