using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.OrganizationStructure.Queries.GetOrganizationNodes;

public sealed record GetOrganizationNodesQuery : IQuery<IReadOnlyCollection<OrganizationNodeDto>>;
