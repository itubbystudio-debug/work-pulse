using MediatR;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.MasterData.Commands.CreateEmployeeGroup;

public sealed class CreateEmployeeGroupCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateEmployeeGroupCommand, Result<CreateEmployeeGroupResponse>>
{
    public async Task<Result<CreateEmployeeGroupResponse>> Handle(
        CreateEmployeeGroupCommand request,
        CancellationToken cancellationToken)
    {
        var employeeGroup = new EmployeeGroup(request.Name, request.Description, request.IsActive);

        dbContext.EmployeeGroups.Add(employeeGroup);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateEmployeeGroupResponse>.Success(new CreateEmployeeGroupResponse(
            employeeGroup.Id,
            employeeGroup.Name,
            employeeGroup.Description,
            employeeGroup.IsActive));
    }
}
