using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.UpdateEmployeeGroup;

public sealed record UpdateEmployeeGroupCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive) : ICommand<UpdateEmployeeGroupResponse>;
