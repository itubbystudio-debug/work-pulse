using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Application.Features.OrganizationStructure;

namespace WorkPulse.Application.Features.OrganizationStructure.Commands.UpdateOrganizationNode;

public sealed class UpdateOrganizationNodeCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateOrganizationNodeCommand, Result<UpdateOrganizationNodeResponse>>
{
    public async Task<Result<UpdateOrganizationNodeResponse>> Handle(
        UpdateOrganizationNodeCommand request,
        CancellationToken cancellationToken)
    {
        var node = await dbContext.OrganizationNodes
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (node is null)
        {
            return Result<UpdateOrganizationNodeResponse>.Failure(OrganizationHierarchyValidator.NotFound);
        }

        var parentError = await OrganizationHierarchyValidator.ValidateParentExistsAsync(
            dbContext,
            request.ParentId,
            cancellationToken);

        if (parentError is not null)
        {
            return Result<UpdateOrganizationNodeResponse>.Failure(parentError);
        }

        var hierarchyError = await OrganizationHierarchyValidator.ValidateNoLoopAsync(
            dbContext,
            request.Id,
            request.ParentId,
            cancellationToken);

        if (hierarchyError is not null)
        {
            return Result<UpdateOrganizationNodeResponse>.Failure(hierarchyError);
        }

        node.Update(request.Name, request.ParentId);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateOrganizationNodeResponse>.Success(
            new UpdateOrganizationNodeResponse(node.Id, node.Name, node.ParentId));
    }
}
