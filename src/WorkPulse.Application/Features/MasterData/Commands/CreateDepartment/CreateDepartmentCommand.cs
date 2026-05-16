using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.CreateDepartment;

public sealed record CreateDepartmentCommand(
    string Name,
    string? Description,
    bool IsActive) : ICommand<CreateDepartmentResponse>;
