using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Queries.GetEmployeeGroupById;

public sealed record GetEmployeeGroupByIdQuery(Guid Id) : IQuery<EmployeeGroupDetailsDto>;

public sealed record EmployeeGroupDetailsDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
