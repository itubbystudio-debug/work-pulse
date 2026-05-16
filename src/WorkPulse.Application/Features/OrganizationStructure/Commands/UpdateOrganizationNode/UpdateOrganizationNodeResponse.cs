namespace WorkPulse.Application.Features.OrganizationStructure.Commands.UpdateOrganizationNode;

public sealed record UpdateOrganizationNodeResponse(Guid Id, string Name, Guid? ParentId);
