using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class Position : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private Position()
    {
        Name = string.Empty;
    }

    public Position(Guid departmentId, string name, string? description, bool isActive)
    {
        Name = string.Empty;
        SetDetails(departmentId, name, description, isActive);
    }

    public Guid DepartmentId { get; private set; }

    public Department Department { get; private set; } = null!;

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

    public void Update(Guid departmentId, string name, string? description, bool isActive)
    {
        SetDetails(departmentId, name, description, isActive);
    }

    private void SetDetails(Guid departmentId, string name, string? description, bool isActive)
    {
        if (departmentId == Guid.Empty)
        {
            throw new BusinessRuleViolationException("Position.DepartmentRequired", "Department is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("Position.NameRequired", "Position name is required.");
        }

        DepartmentId = departmentId;
        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        IsActive = isActive;
    }
}
