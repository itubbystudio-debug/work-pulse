using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.UpdatePosition;

public sealed record UpdatePositionCommand(
    Guid Id,
    Guid DepartmentId,
    string Name,
    string? Description,
    bool IsActive) : ICommand<UpdatePositionResponse>;
