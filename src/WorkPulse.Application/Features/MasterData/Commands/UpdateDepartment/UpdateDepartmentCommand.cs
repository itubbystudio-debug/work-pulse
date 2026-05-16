using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Commands.UpdateDepartment;

public sealed record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive) : ICommand<UpdateDepartmentResponse>;
