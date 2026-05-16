using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class EmployeeGroup : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private EmployeeGroup()
    {
        Name = string.Empty;
    }

    public EmployeeGroup(string name, string? description, bool isActive)
    {
        Name = string.Empty;
        SetDetails(name, description, isActive);
    }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public void Update(string name, string? description, bool isActive)
    {
        SetDetails(name, description, isActive);
    }

    private void SetDetails(string name, string? description, bool isActive)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("EmployeeGroup.NameRequired", "Employee group name is required.");
        }

        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        IsActive = isActive;
    }
}
