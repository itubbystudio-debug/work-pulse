namespace WorkPulse.Application.Features.OrganizationStructure.Queries.GetOrganizationNodes;

public sealed record OrganizationNodeDto(Guid Id, string Name, Guid? ParentId);
