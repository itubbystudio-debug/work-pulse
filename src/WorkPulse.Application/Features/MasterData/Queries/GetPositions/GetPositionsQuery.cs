using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.MasterData.Queries.GetPositions;

public sealed record GetPositionsQuery(Guid? DepartmentId = null) : IQuery<IReadOnlyCollection<PositionDto>>;

public sealed record PositionDto(
    Guid Id,
    Guid DepartmentId,
    string DepartmentName,
    string Name,
    string? Description,
    bool IsActive);
