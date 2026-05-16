namespace WorkPulse.Application.Features.OrganizationStructure.Commands.CreateOrganizationNode;

public sealed record CreateOrganizationNodeResponse(Guid Id, string Name, Guid? ParentId);
