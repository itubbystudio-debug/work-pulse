using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class OrganizationNode : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private OrganizationNode()
    {
        Name = string.Empty;
    }

    public OrganizationNode(string name, Guid? parentId)
    {
        SetName(name);
        SetParent(parentId);
    }

    public string Name { get; private set; } = string.Empty;

    public Guid? ParentId { get; private set; }

    public OrganizationNode? Parent { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public void Update(string name, Guid? parentId)
    {
        SetName(name);
        SetParent(parentId);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException(
                "OrganizationNode.NameRequired",
                "Organization node name is required.");
        }

        Name = name.Trim();
    }

    private void SetParent(Guid? parentId)
    {
        if (parentId == Id)
        {
            throw new BusinessRuleViolationException(
                "OrganizationNode.InvalidParent",
                "Organization node cannot be its own parent.");
        }

        ParentId = parentId;
    }
}
