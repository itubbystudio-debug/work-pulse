using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.WorkTypes.Commands.UpdateWorkType;

public sealed record UpdateWorkTypeCommand(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    bool IsActive,
    string? PolicySettingsJson) : ICommand<UpdateWorkTypeResponse>;
