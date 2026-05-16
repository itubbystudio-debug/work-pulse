using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.DeleteDepartment;

public sealed record DeleteDepartmentCommand(Guid Id) : ICommand;
