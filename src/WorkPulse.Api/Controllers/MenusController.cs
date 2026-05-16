using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.Menus.Commands.CreateMenu;
using WorkPulse.Application.Features.Menus.Commands.UpdateMenu;
using WorkPulse.Application.Features.Menus.Queries.GetMenus;

namespace WorkPulse.Api.Controllers;

[AllowAnonymous]
public sealed class MenusController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> GetMenus(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetMenusQuery(), cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMenu(
        CreateMenuRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateMenuCommand(
            request.Name,
            request.Route,
            request.Icon,
            request.DisplayOrder,
            request.ParentMenuId), cancellationToken);

        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMenu(
        Guid id,
        UpdateMenuRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateMenuCommand(
            id,
            request.Name,
            request.Route,
            request.Icon,
            request.DisplayOrder,
            request.ParentMenuId), cancellationToken);

        return FromResult(result);
    }
}

public sealed record CreateMenuRequest(
    string Name,
    string? Route,
    string? Icon,
    int DisplayOrder,
    Guid? ParentMenuId);

public sealed record UpdateMenuRequest(
    string Name,
    string? Route,
    string? Icon,
    int DisplayOrder,
    Guid? ParentMenuId);
