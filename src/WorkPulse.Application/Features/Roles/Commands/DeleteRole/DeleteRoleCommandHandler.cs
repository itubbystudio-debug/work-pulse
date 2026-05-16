using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Roles.Commands.DeleteRole;

public sealed class DeleteRoleCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<DeleteRoleCommand, Result>
{
    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await dbContext.Roles
            .FirstOrDefaultAsync(role => role.Id == request.Id, cancellationToken);

        if (role is null)
        {
            return Result.Failure(Error.Validation("Role.NotFound", "Role was not found."));
        }

        dbContext.Roles.Remove(role);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
