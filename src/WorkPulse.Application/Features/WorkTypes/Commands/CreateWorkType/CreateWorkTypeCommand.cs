using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.WorkTypes.Commands.CreateWorkType;

public sealed record CreateWorkTypeCommand(
    string Code,
    string Name,
    string? Description,
    bool IsActive,
    string? PolicySettingsJson) : ICommand<CreateWorkTypeResponse>;
