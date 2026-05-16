using WorkPulse.Application.Common.Messaging;
using WorkPulse.Domain.Enums;

namespace WorkPulse.Application.Features.SystemAdministration.Commands.CreateSystemAdministrationRecord;

public sealed record CreateSystemAdministrationRecordCommand(
    SystemAdministrationRecordType Type,
    string Name,
    string? Code,
    string? Description,
    Guid? ParentId,
    bool IsActive) : ICommand<SystemAdministrationRecordResponse>;
