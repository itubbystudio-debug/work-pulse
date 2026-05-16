using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Menus.Queries.GetMenus;

public sealed class GetMenusQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetMenusQuery, Result<IReadOnlyCollection<MenuDto>>>
{
    public async Task<Result<IReadOnlyCollection<MenuDto>>> Handle(
        GetMenusQuery request,
        CancellationToken cancellationToken)
    {
        var menus = await dbContext.NavigationMenus
            .AsNoTracking()
            .OrderBy(menu => menu.ParentMenuId)
            .ThenBy(menu => menu.DisplayOrder)
            .ThenBy(menu => menu.Name)
            .Select(menu => new MenuListItem(
                menu.Id,
                menu.Name,
                menu.Route,
                menu.Icon,
                menu.DisplayOrder,
                menu.ParentMenuId))
            .ToListAsync(cancellationToken);

        var childrenByParentId = menus
            .Where(menu => menu.ParentMenuId.HasValue)
            .GroupBy(menu => menu.ParentMenuId!.Value)
            .ToDictionary(group => group.Key, group => group.ToArray());

        var menuIds = menus.Select(menu => menu.Id).ToHashSet();
        var rootMenus = menus
            .Where(menu => !menu.ParentMenuId.HasValue || !menuIds.Contains(menu.ParentMenuId.Value))
            .Select(menu => ToDto(menu, childrenByParentId))
            .ToArray();

        return Result<IReadOnlyCollection<MenuDto>>.Success(rootMenus);
    }

    private static MenuDto ToDto(
        MenuListItem menu,
        IReadOnlyDictionary<Guid, MenuListItem[]> childrenByParentId)
    {
        var children = childrenByParentId.TryGetValue(menu.Id, out var childMenus)
            ? childMenus.Select(child => ToDto(child, childrenByParentId)).ToArray()
            : [];

        return new MenuDto(
            menu.Id,
            menu.Name,
            menu.Route,
            menu.Icon,
            menu.DisplayOrder,
            menu.ParentMenuId,
            children);
    }

    private sealed record MenuListItem(
        Guid Id,
        string Name,
        string? Route,
        string? Icon,
        int DisplayOrder,
        Guid? ParentMenuId);
}
