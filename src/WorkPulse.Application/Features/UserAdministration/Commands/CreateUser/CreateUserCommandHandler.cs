using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.UserAdministration.Commands.CreateUser;

public sealed class CreateUserCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateUserCommand, Result<CreateUserResponse>>
{
    public async Task<Result<CreateUserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        var duplicateExists = await dbContext.SystemUsers
            .AsNoTracking()
            .AnyAsync(
                user => user.IdentityUserId == request.IdentityUserId
                    || user.UserName == request.UserName
                    || user.Email == request.Email,
                cancellationToken);

        if (duplicateExists)
        {
            return Result<CreateUserResponse>.Failure(
                new Error("SystemUser.Duplicate", "A user with the same identity id, username, or email already exists."));
        }

        var user = SystemUser.Create(
            request.IdentityUserId,
            request.UserName,
            request.Email,
            request.DisplayName,
            request.Role,
            request.IsActive);

        dbContext.SystemUsers.Add(user);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateUserResponse>.Success(new CreateUserResponse(
            user.Id,
            user.IdentityUserId,
            user.UserName,
            user.Email,
            user.DisplayName,
            user.Role,
            user.IsActive));
    }
}
