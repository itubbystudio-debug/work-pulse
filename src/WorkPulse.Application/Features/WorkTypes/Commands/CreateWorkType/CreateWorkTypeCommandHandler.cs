using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.WorkTypes.Commands.CreateWorkType;

public sealed class CreateWorkTypeCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateWorkTypeCommand, Result<CreateWorkTypeResponse>>
{
    public async Task<Result<CreateWorkTypeResponse>> Handle(
        CreateWorkTypeCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedCode = request.Code.Trim().ToUpperInvariant();
        var codeExists = await dbContext.WorkTypes
            .AsNoTracking()
            .AnyAsync(workType => workType.Code == normalizedCode, cancellationToken);

        if (codeExists)
        {
            return Result<CreateWorkTypeResponse>.Failure(
                new Error("WorkType.CodeAlreadyExists", "Work type code already exists."));
        }

        var workType = new WorkType(
            request.Code,
            request.Name,
            request.Description,
            request.IsActive,
            request.PolicySettingsJson);

        dbContext.WorkTypes.Add(workType);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateWorkTypeResponse>.Success(new CreateWorkTypeResponse(
            workType.Id,
            workType.Code,
            workType.Name,
            workType.Description,
            workType.IsActive,
            workType.PolicySettingsJson));
    }
}
