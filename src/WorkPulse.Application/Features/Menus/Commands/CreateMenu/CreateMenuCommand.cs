using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.Menus.Commands.CreateMenu;

public sealed record CreateMenuCommand(
    string Name,
    string? Route,
    string? Icon,
    int DisplayOrder,
    Guid? ParentMenuId) : ICommand<CreateMenuResponse>;
