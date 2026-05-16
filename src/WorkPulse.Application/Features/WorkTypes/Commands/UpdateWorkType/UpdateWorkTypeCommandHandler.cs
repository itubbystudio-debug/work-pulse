using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.WorkTypes.Commands.UpdateWorkType;

public sealed class UpdateWorkTypeCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateWorkTypeCommand, Result<UpdateWorkTypeResponse>>
{
    public async Task<Result<UpdateWorkTypeResponse>> Handle(
        UpdateWorkTypeCommand request,
        CancellationToken cancellationToken)
    {
        var workType = await dbContext.WorkTypes
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (workType is null)
        {
            return Result<UpdateWorkTypeResponse>.Failure(
                new Error("WorkType.NotFound", "Work type was not found."));
        }

        var normalizedCode = request.Code.Trim().ToUpperInvariant();
        var codeExists = await dbContext.WorkTypes
            .AsNoTracking()
            .AnyAsync(item => item.Id != request.Id && item.Code == normalizedCode, cancellationToken);

        if (codeExists)
        {
            return Result<UpdateWorkTypeResponse>.Failure(
                new Error("WorkType.CodeAlreadyExists", "Work type code already exists."));
        }

        workType.Update(
            request.Code,
            request.Name,
            request.Description,
            request.IsActive,
            request.PolicySettingsJson);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateWorkTypeResponse>.Success(new UpdateWorkTypeResponse(
            workType.Id,
            workType.Code,
            workType.Name,
            workType.Description,
            workType.IsActive,
            workType.PolicySettingsJson));
    }
}
