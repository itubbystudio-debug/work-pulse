using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Queries.GetDepartments;

public sealed record GetDepartmentsQuery : IQuery<IReadOnlyCollection<DepartmentDto>>;

public sealed record DepartmentDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
