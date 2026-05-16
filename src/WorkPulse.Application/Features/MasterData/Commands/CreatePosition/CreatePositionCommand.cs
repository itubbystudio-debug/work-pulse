using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.CreatePosition;

public sealed record CreatePositionCommand(
    Guid DepartmentId,
    string Name,
    string? Description,
    bool IsActive) : ICommand<CreatePositionResponse>;
