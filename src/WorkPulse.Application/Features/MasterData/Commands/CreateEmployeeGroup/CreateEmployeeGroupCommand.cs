using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.CreateEmployeeGroup;

public sealed record CreateEmployeeGroupCommand(
    string Name,
    string? Description,
    bool IsActive) : ICommand<CreateEmployeeGroupResponse>;
