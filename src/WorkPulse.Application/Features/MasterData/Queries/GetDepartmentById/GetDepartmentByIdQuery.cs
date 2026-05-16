using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Queries.GetDepartmentById;

public sealed record GetDepartmentByIdQuery(Guid Id) : IQuery<DepartmentDetailsDto>;

public sealed record DepartmentDetailsDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive);
