using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class NavigationMenu : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private readonly List<NavigationMenu> _children = [];

    private NavigationMenu()
    {
        Name = string.Empty;
    }

    public NavigationMenu(
        string name,
        string? route,
        string? icon,
        int displayOrder,
        Guid? parentMenuId)
    {
        SetDetails(name, route, icon, displayOrder);
        SetParent(parentMenuId);
    }

    public string Name { get; private set; } = string.Empty;

    public string? Route { get; private set; }

    public string? Icon { get; private set; }

    public int DisplayOrder { get; private set; }

    public Guid? ParentMenuId { get; private set; }

    public NavigationMenu? ParentMenu { get; private set; }

    public IReadOnlyCollection<NavigationMenu> Children => _children.AsReadOnly();

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public void Update(
        string name,
        string? route,
        string? icon,
        int displayOrder,
        Guid? parentMenuId)
    {
        SetDetails(name, route, icon, displayOrder);
        SetParent(parentMenuId);
    }

    private void SetDetails(string name, string? route, string? icon, int displayOrder)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("Menu.NameRequired", "Menu name is required.");
        }

        Name = name.Trim();
        Route = string.IsNullOrWhiteSpace(route) ? null : route.Trim();
        Icon = string.IsNullOrWhiteSpace(icon) ? null : icon.Trim();
        DisplayOrder = displayOrder;
    }

    private void SetParent(Guid? parentMenuId)
    {
        if (parentMenuId == Id)
        {
            throw new BusinessRuleViolationException("Menu.InvalidParent", "A menu cannot be its own parent.");
        }

        ParentMenuId = parentMenuId;
    }
}
