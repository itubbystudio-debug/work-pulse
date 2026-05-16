namespace WorkPulse.Application.Features.Menus.Commands.UpdateMenu;

public sealed record UpdateMenuResponse(
    Guid Id,
    string Name,
    string? Route,
    string? Icon,
    int DisplayOrder,
    Guid? ParentMenuId);
