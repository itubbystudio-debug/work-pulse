using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.DeleteEmployeeGroup;

public sealed record DeleteEmployeeGroupCommand(Guid Id) : ICommand;
