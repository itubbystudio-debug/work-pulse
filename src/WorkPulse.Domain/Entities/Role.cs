using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class Role : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private Role()
    {
        Code = string.Empty;
        Name = string.Empty;
    }

    public Role(string code, string name, string? description, bool isActive = true)
    {
        Name = string.Empty;

        if (string.IsNullOrWhiteSpace(code))
        {
            throw new BusinessRuleViolationException("Role.CodeRequired", "Role code is required.");
        }

        Code = code.Trim();
        Rename(name);
        UpdateDescription(description);
        IsActive = isActive;
    }

    public string Code { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public void Update(string name, string? description, bool isActive)
    {
        Rename(name);
        UpdateDescription(description);
        IsActive = isActive;
    }

    private void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("Role.NameRequired", "Role name is required.");
        }

        Name = name.Trim();
    }

    private void UpdateDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
    }
}
