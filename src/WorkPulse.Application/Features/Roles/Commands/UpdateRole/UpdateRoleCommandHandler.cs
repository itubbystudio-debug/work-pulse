using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Roles.Commands.UpdateRole;

public sealed class UpdateRoleCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateRoleCommand, Result<UpdateRoleResponse>>
{
    public async Task<Result<UpdateRoleResponse>> Handle(
        UpdateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .FirstOrDefaultAsync(role => role.Id == request.Id, cancellationToken);

        if (role is null)
        {
            return Result<UpdateRoleResponse>.Failure(
                Error.Validation("Role.NotFound", "Role was not found."));
        }

        var name = request.Name.Trim();
        var normalizedName = name.ToUpperInvariant();
        var duplicateNameExists = await dbContext.Roles
            .AnyAsync(
                candidate => candidate.Id != request.Id && candidate.Name.ToUpper() == normalizedName,
                cancellationToken);

        if (duplicateNameExists)
        {
            return Result<UpdateRoleResponse>.Failure(
                Error.Validation("Role.NameDuplicate", "Role name already exists."));
        }

        role.Update(name, request.Description, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateRoleResponse>.Success(new UpdateRoleResponse(
            role.Id,
            role.Code,
            role.Name,
            role.Description,
            role.IsActive));
    }
}
