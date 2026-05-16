using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.Menus.Commands.CreateMenu;

public sealed class CreateMenuCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateMenuCommand, Result<CreateMenuResponse>>
{
    public async Task<Result<CreateMenuResponse>> Handle(
        CreateMenuCommand request,
        CancellationToken cancellationToken)
    {
        if (request.ParentMenuId.HasValue)
        {
            var parentExists = await dbContext.NavigationMenus
                .AsNoTracking()
                .AnyAsync(menu => menu.Id == request.ParentMenuId.Value, cancellationToken);

            if (!parentExists)
            {
                return Result<CreateMenuResponse>.Failure(
                    new Error("Menu.ParentNotFound", "Parent menu was not found."));
            }
        }

        var menu = new NavigationMenu(
            request.Name,
            request.Route,
            request.Icon,
            request.DisplayOrder,
            request.ParentMenuId);

        dbContext.NavigationMenus.Add(menu);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateMenuResponse>.Success(new CreateMenuResponse(
            menu.Id,
            menu.Name,
            menu.Route,
            menu.Icon,
            menu.DisplayOrder,
            menu.ParentMenuId));
    }
}
