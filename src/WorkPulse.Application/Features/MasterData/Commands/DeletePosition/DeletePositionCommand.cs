using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.DeletePosition;

public sealed record DeletePositionCommand(Guid Id) : ICommand;
