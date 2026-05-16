using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.UpdateSystemAdministrationRecord;

public sealed class UpdateSystemAdministrationRecordCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateSystemAdministrationRecordCommand, Result<UpdateSystemAdministrationRecordResponse>>
{
    public async Task<Result<UpdateSystemAdministrationRecordResponse>> Handle(
        UpdateSystemAdministrationRecordCommand request,
        CancellationToken cancellationToken)
    {
        var record = await dbContext.SystemAdministrationRecords
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (record is null)
        {
            return Result<UpdateSystemAdministrationRecordResponse>.Failure(
                new Error("SystemAdministration.NotFound", "Configuration record was not found."));
        }

        if (request.ParentId == request.Id)
        {
            return Result<UpdateSystemAdministrationRecordResponse>.Failure(
                new Error("SystemAdministration.InvalidParent", "A configuration record cannot be its own parent."));
        }

        if (request.ParentId.HasValue)
        {
            var parentExists = await dbContext.SystemAdministrationRecords
                .AsNoTracking()
                .AnyAsync(item => item.Id == request.ParentId.Value, cancellationToken);

            if (!parentExists)
            {
                return Result<UpdateSystemAdministrationRecordResponse>.Failure(
                    new Error("SystemAdministration.ParentNotFound", "Parent configuration record was not found."));
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var normalizedCode = request.Code.Trim();
            var codeExists = await dbContext.SystemAdministrationRecords
                .AsNoTracking()
                .AnyAsync(
                    item => item.Id != request.Id && item.Type == record.Type && item.Code == normalizedCode,
                    cancellationToken);

            if (codeExists)
            {
                return Result<UpdateSystemAdministrationRecordResponse>.Failure(
                    new Error("SystemAdministration.CodeAlreadyExists", "A configuration record with this code already exists."));
            }
        }

        record.Update(request.Name, request.Code, request.Description, request.ParentId, request.IsActive);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateSystemAdministrationRecordResponse>.Success(new UpdateSystemAdministrationRecordResponse(
            record.Id,
            record.Type,
            record.Name,
            record.Code,
            record.Description,
            record.ParentId,
            record.IsActive));
    }
}
