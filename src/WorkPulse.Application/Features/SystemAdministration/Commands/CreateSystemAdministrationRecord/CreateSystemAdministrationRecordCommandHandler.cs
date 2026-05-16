using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.CreateSystemAdministrationRecord;

public sealed class CreateSystemAdministrationRecordCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateSystemAdministrationRecordCommand, Result<SystemAdministrationRecordResponse>>
{
    public async Task<Result<SystemAdministrationRecordResponse>> Handle(
        CreateSystemAdministrationRecordCommand request,
        CancellationToken cancellationToken)
    {
        if (request.ParentId.HasValue)
        {
            var parentExists = await dbContext.SystemAdministrationRecords
                .AsNoTracking()
                .AnyAsync(record => record.Id == request.ParentId.Value, cancellationToken);

            if (!parentExists)
            {
                return Result<SystemAdministrationRecordResponse>.Failure(
                    new Error("SystemAdministration.ParentNotFound", "Parent configuration record was not found."));
            }
        }

        if (!string.IsNullOrWhiteSpace(request.Code))
        {
            var codeExists = await dbContext.SystemAdministrationRecords
                .AsNoTracking()
                .AnyAsync(
                    record => record.Type == request.Type && record.Code == request.Code.Trim(),
                    cancellationToken);

            if (codeExists)
            {
                return Result<SystemAdministrationRecordResponse>.Failure(
                    new Error("SystemAdministration.CodeAlreadyExists", "A configuration record with this code already exists."));
            }
        }

        var record = SystemAdministrationRecord.Create(
            request.Type,
            request.Name,
            request.Code,
            request.Description,
            request.ParentId,
            request.IsActive);

        dbContext.SystemAdministrationRecords.Add(record);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<SystemAdministrationRecordResponse>.Success(new SystemAdministrationRecordResponse(
            record.Id,
            record.Type,
            record.Name,
            record.Code,
            record.Description,
            record.ParentId,
            record.IsActive));
    }
}
