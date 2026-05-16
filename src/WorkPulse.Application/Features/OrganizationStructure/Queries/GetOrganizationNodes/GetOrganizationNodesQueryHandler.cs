using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.OrganizationStructure.Queries.GetOrganizationNodes;

public sealed class GetOrganizationNodesQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetOrganizationNodesQuery, Result<IReadOnlyCollection<OrganizationNodeDto>>>
{
    public async Task<Result<IReadOnlyCollection<OrganizationNodeDto>>> Handle(
        GetOrganizationNodesQuery request,
        CancellationToken cancellationToken)
    {
        var nodes = await dbContext.OrganizationNodes
            .AsNoTracking()
            .OrderBy(node => node.ParentId.HasValue)
            .ThenBy(node => node.Name)
            .Select(node => new OrganizationNodeDto(node.Id, node.Name, node.ParentId))
            .ToArrayAsync(cancellationToken);

        return Result<IReadOnlyCollection<OrganizationNodeDto>>.Success(nodes);
    }
}
