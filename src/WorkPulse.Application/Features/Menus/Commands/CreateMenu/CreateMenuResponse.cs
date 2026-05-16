namespace WorkPulse.Application.Features.Menus.Commands.CreateMenu;

public sealed record CreateMenuResponse(
    Guid Id,
    string Name,
    string? Route,
    string? Icon,
    int DisplayOrder,
    Guid? ParentMenuId);
