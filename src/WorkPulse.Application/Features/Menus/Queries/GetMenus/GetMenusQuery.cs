using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Menus.Queries.GetMenus;

public sealed record GetMenusQuery() : IQuery<IReadOnlyCollection<MenuDto>>;

public sealed record MenuDto(
    Guid Id,
    string Name,
    string? Route,
    string? Icon,
    int DisplayOrder,
    Guid? ParentMenuId,
    IReadOnlyCollection<MenuDto> Children);
