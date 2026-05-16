using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.UserAdministration.Commands.SetUserActivation;

public sealed class SetUserActivationCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<SetUserActivationCommand, Result<SetUserActivationResponse>>
{
    public async Task<Result<SetUserActivationResponse>> Handle(
        SetUserActivationCommand request,
        CancellationToken cancellationToken)
    {
        var user = await dbContext.SystemUsers
            .FirstOrDefaultAsync(systemUser => systemUser.Id == request.Id, cancellationToken);

        if (user is null)
        {
            return Result<SetUserActivationResponse>.Failure(new Error("SystemUser.NotFound", "User was not found."));
        }

        user.SetActivation(request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<SetUserActivationResponse>.Success(new SetUserActivationResponse(user.Id, user.IsActive));
    }
}
