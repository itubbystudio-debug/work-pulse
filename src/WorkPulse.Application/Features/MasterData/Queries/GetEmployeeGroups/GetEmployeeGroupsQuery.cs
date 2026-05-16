using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Queries.GetEmployeeGroups;

public sealed record GetEmployeeGroupsQuery : IQuery<IReadOnlyCollection<EmployeeGroupDto>>;

public sealed record EmployeeGroupDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
