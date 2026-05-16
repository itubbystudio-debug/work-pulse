using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Menus.Commands.UpdateMenu;

public sealed class UpdateMenuCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateMenuCommand, Result<UpdateMenuResponse>>
{
    public async Task<Result<UpdateMenuResponse>> Handle(
        UpdateMenuCommand request,
        CancellationToken cancellationToken)
    {
        var menu = await dbContext.NavigationMenus
            .FirstOrDefaultAsync(item => item.Id == request.Id, cancellationToken);

        if (menu is null)
        {
            return Result<UpdateMenuResponse>.Failure(
                new Error("Menu.NotFound", "Menu was not found."));
        }

        var parentError = await ValidateParentAsync(
            request.Id,
            request.ParentMenuId,
            cancellationToken);

        if (parentError is not null)
        {
            return Result<UpdateMenuResponse>.Failure(parentError);
        }

        menu.Update(
            request.Name,
            request.Route,
            request.Icon,
            request.DisplayOrder,
            request.ParentMenuId);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateMenuResponse>.Success(new UpdateMenuResponse(
            menu.Id,
            menu.Name,
            menu.Route,
            menu.Icon,
            menu.DisplayOrder,
            menu.ParentMenuId));
    }

    private async Task<Error?> ValidateParentAsync(
        Guid menuId,
        Guid? parentMenuId,
        CancellationToken cancellationToken)
    {
        if (!parentMenuId.HasValue)
        {
            return null;
        }

        if (parentMenuId.Value == menuId)
        {
            return new Error("Menu.InvalidParent", "A menu cannot be its own parent.");
        }

        var menus = await dbContext.NavigationMenus
            .AsNoTracking()
            .Select(menu => new MenuParentReference(menu.Id, menu.ParentMenuId))
            .ToListAsync(cancellationToken);

        var parentLookup = menus.ToDictionary(menu => menu.Id, menu => menu.ParentMenuId);
        if (!parentLookup.ContainsKey(parentMenuId.Value))
        {
            return new Error("Menu.ParentNotFound", "Parent menu was not found.");
        }

        var visited = new HashSet<Guid>();
        var currentParentId = parentMenuId;
        while (currentParentId.HasValue)
        {
            if (currentParentId.Value == menuId)
            {
                return new Error("Menu.InvalidParent", "A menu cannot be assigned under its own sub-menu.");
            }

            if (!visited.Add(currentParentId.Value)
                || !parentLookup.TryGetValue(currentParentId.Value, out var nextParentId))
            {
                break;
            }

            currentParentId = nextParentId;
        }

        return null;
    }

    private sealed record MenuParentReference(Guid Id, Guid? ParentMenuId);
}
