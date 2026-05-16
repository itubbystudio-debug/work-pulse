using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateRoleCommand, Result<CreateRoleResponse>>
{
    public async Task<Result<CreateRoleResponse>> Handle(
        CreateRoleCommand request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.Trim();
        var name = request.Name.Trim();
        var normalizedCode = code.ToUpperInvariant();
        var normalizedName = name.ToUpperInvariant();

        var duplicateCodeExists = await dbContext.Roles
            .AnyAsync(role => role.Code.ToUpper() == normalizedCode, cancellationToken);

        if (duplicateCodeExists)
        {
            return Result<CreateRoleResponse>.Failure(
                Error.Validation("Role.CodeDuplicate", "Role code already exists."));
        }

        var duplicateNameExists = await dbContext.Roles
            .AnyAsync(role => role.Name.ToUpper() == normalizedName, cancellationToken);

        if (duplicateNameExists)
        {
            return Result<CreateRoleResponse>.Failure(
                Error.Validation("Role.NameDuplicate", "Role name already exists."));
        }

        var role = new Role(code, name, request.Description, request.IsActive);

        dbContext.Roles.Add(role);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateRoleResponse>.Success(new CreateRoleResponse(
            role.Id,
            role.Code,
            role.Name,
            role.Description,
            role.IsActive));
    }
}
