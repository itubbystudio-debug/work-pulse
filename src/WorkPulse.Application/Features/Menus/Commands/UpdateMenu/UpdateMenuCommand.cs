using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Menus.Commands.UpdateMenu;

public sealed record UpdateMenuCommand(
    Guid Id,
    string Name,
    string? Route,
    string? Icon,
    int DisplayOrder,
    Guid? ParentMenuId) : ICommand<UpdateMenuResponse>;
