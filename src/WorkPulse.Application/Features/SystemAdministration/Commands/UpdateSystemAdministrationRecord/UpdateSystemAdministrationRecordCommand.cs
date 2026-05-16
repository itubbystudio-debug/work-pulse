using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.UpdateSystemAdministrationRecord;

public sealed record UpdateSystemAdministrationRecordCommand(
    Guid Id,
    string Name,
    string? Code,
    string? Description,
    Guid? ParentId,
    bool IsActive) : ICommand<UpdateSystemAdministrationRecordResponse>;
