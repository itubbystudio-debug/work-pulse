using MediatR;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Application.Features.OrganizationStructure;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.OrganizationStructure.Commands.CreateOrganizationNode;

public sealed class CreateOrganizationNodeCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateOrganizationNodeCommand, Result<CreateOrganizationNodeResponse>>
{
    public async Task<Result<CreateOrganizationNodeResponse>> Handle(
        CreateOrganizationNodeCommand request,
        CancellationToken cancellationToken)
    {
        var parentError = await OrganizationHierarchyValidator.ValidateParentExistsAsync(
            dbContext,
            request.ParentId,
            cancellationToken);

        if (parentError is not null)
        {
            return Result<CreateOrganizationNodeResponse>.Failure(parentError);
        }

        var node = new OrganizationNode(request.Name, request.ParentId);

        dbContext.OrganizationNodes.Add(node);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateOrganizationNodeResponse>.Success(
            new CreateOrganizationNodeResponse(node.Id, node.Name, node.ParentId));
    }
}
