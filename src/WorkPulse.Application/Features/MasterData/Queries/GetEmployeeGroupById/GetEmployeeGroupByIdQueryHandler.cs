using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.MasterData.Queries.GetEmployeeGroupById;

public sealed class GetEmployeeGroupByIdQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetEmployeeGroupByIdQuery, Result<EmployeeGroupDetailsDto>>
{
    public async Task<Result<EmployeeGroupDetailsDto>> Handle(
        GetEmployeeGroupByIdQuery request,
        CancellationToken cancellationToken)
    {
        var employeeGroup = await dbContext.EmployeeGroups
            .AsNoTracking()
            .Where(item => item.Id == request.Id)
            .Select(item => new EmployeeGroupDetailsDto(
                item.Id,
                item.Name,
                item.Description,
                item.IsActive))
            .FirstOrDefaultAsync(cancellationToken);

        return employeeGroup is null
            ? Result<EmployeeGroupDetailsDto>.Failure(new Error("EmployeeGroup.NotFound", "Employee group was not found."))
            : Result<EmployeeGroupDetailsDto>.Success(employeeGroup);
    }
}
