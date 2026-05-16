using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.OrganizationStructure.Commands.UpdateOrganizationNode;

public sealed record UpdateOrganizationNodeCommand(Guid Id, string Name, Guid? ParentId)
    : ICommand<UpdateOrganizationNodeResponse>;
