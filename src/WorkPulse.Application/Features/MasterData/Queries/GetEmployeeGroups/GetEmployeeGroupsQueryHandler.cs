using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Queries.GetEmployeeGroups;

public sealed class GetEmployeeGroupsQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetEmployeeGroupsQuery, Result<IReadOnlyCollection<EmployeeGroupDto>>>
{
    public async Task<Result<IReadOnlyCollection<EmployeeGroupDto>>> Handle(
        GetEmployeeGroupsQuery request,
        CancellationToken cancellationToken)
    {
        var employeeGroups = await dbContext.EmployeeGroups
            .AsNoTracking()
            .OrderBy(employeeGroup => employeeGroup.Name)
            .Select(employeeGroup => new EmployeeGroupDto(
                employeeGroup.Id,
                employeeGroup.Name,
                employeeGroup.Description,
                employeeGroup.IsActive))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<EmployeeGroupDto>>.Success(employeeGroups);
    }
}
