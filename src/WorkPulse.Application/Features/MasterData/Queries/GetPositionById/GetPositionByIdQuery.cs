using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Queries.GetPositionById;

public sealed record GetPositionByIdQuery(Guid Id) : IQuery<PositionDetailsDto>;

public sealed record PositionDetailsDto(
    Guid Id,
    Guid DepartmentId,
    string DepartmentName,
    string Name,
    string? Description,
    bool IsActive);
