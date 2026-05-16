using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.OrganizationStructure;

internal static class OrganizationHierarchyValidator
{
    public static readonly Error ParentNotFound = new(
        "OrganizationNode.ParentNotFound",
        "Organization parent node was not found.");

    public static readonly Error NotFound = new(
        "OrganizationNode.NotFound",
        "Organization node was not found.");

    public static readonly Error InvalidHierarchy = new(
        "OrganizationNode.InvalidHierarchy",
        "Organization hierarchy cannot contain a parent-child loop.");

    public static async Task<Error?> ValidateParentExistsAsync(
        IApplicationDbContext dbContext,
        Guid? parentId,
        CancellationToken cancellationToken)
    {
        if (parentId is null)
        {
            return null;
        }

        var parentExists = await dbContext.OrganizationNodes
            .AsNoTracking()
            .AnyAsync(node => node.Id == parentId.Value, cancellationToken);

        return parentExists ? null : ParentNotFound;
    }

    public static async Task<Error?> ValidateNoLoopAsync(
        IApplicationDbContext dbContext,
        Guid nodeId,
        Guid? proposedParentId,
        CancellationToken cancellationToken)
    {
        if (proposedParentId is null)
        {
            return null;
        }

        if (proposedParentId == nodeId)
        {
            return InvalidHierarchy;
        }

        var currentId = proposedParentId;
        var visited = new HashSet<Guid>();

        while (currentId is not null)
        {
            if (!visited.Add(currentId.Value))
            {
                return InvalidHierarchy;
            }

            if (currentId == nodeId)
            {
                return InvalidHierarchy;
            }

            currentId = await dbContext.OrganizationNodes
                .AsNoTracking()
                .Where(node => node.Id == currentId.Value)
                .Select(node => node.ParentId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return null;
    }
}
