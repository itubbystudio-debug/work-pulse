using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Queries.GetDepartmentById;

public sealed class GetDepartmentByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDetailsDto>>
{
    public async Task<Result<DepartmentDetailsDto>> Handle(
        GetDepartmentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var department = await dbContext.Departments
            .AsNoTracking()
            .Where(item => item.Id == request.Id)
            .Select(item => new DepartmentDetailsDto(
                item.Id,
                item.Name,
                item.Description,
                item.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        return department is null
            ? Result<DepartmentDetailsDto>.Failure(new Error("Department.NotFound", "Department was not found."))
            : Result<DepartmentDetailsDto>.Success(department);
    }
}
