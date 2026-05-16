using MediatR;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.MasterData.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateDepartmentCommand, Result<CreateDepartmentResponse>>
{
    public async Task<Result<CreateDepartmentResponse>> Handle(
        CreateDepartmentCommand request,
        CancellationToken cancellationToken)
    {
        var department = new Department(request.Name, request.Description, request.IsActive);

        dbContext.Departments.Add(department);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateDepartmentResponse>.Success(new CreateDepartmentResponse(
            department.Id,
            department.Name,
            department.Description,
            department.IsActive));
    }
}
