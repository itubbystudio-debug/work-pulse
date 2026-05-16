using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.UserAdministration.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateUserCommand, Result<UpdateUserResponse>>
{
    public async Task<Result<UpdateUserResponse>> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.SystemUsers
            .FirstOrDefaultAsync(systemUser => systemUser.Id == request.Id, cancellationToken);

        if (user is null)
        {
            return Result<UpdateUserResponse>.Failure(new Error("SystemUser.NotFound", "User was not found."));
        }

        var duplicateExists = await dbContext.SystemUsers
            .AsNoTracking()
            .AnyAsync(
                systemUser => systemUser.Id != request.Id
                    && (systemUser.UserName == request.UserName || systemUser.Email == request.Email),
                cancellationToken);

        if (duplicateExists)
        {
            return Result<UpdateUserResponse>.Failure(
                new Error("SystemUser.Duplicate", "A user with the same username or email already exists."));
        }

        user.UpdateProfile(request.UserName, request.Email, request.DisplayName, request.Role);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateUserResponse>.Success(new UpdateUserResponse(
            user.Id,
            user.IdentityUserId,
            user.UserName,
            user.Email,
            user.DisplayName,
            user.Role,
            user.IsActive));
    }
}
