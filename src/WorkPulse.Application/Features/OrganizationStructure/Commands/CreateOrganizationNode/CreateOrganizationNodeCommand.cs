using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.OrganizationStructure.Commands.CreateOrganizationNode;

public sealed record CreateOrganizationNodeCommand(string Name, Guid? ParentId)
    : ICommand<CreateOrganizationNodeResponse>;
