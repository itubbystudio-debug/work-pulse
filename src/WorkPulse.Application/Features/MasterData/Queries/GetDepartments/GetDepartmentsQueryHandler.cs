using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Queries.GetDepartments;

public sealed class GetDepartmentsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetDepartmentsQuery, Result<IReadOnlyCollection<DepartmentDto>>>
{
    public async Task<Result<IReadOnlyCollection<DepartmentDto>>> Handle(
        GetDepartmentsQuery request,
        CancellationToken cancellationToken)
    {
        var departments = await dbContext.Departments
            .AsNoTracking()
            .OrderBy(department => department.Name)
            .Select(department => new DepartmentDto(
                department.Id,
                department.Name,
                department.Description,
                department.IsActive))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<DepartmentDto>>.Success(departments);
    }
}
