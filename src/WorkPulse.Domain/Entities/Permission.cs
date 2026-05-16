using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class Permission : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private Permission()
    {
        Code = string.Empty;
        Name = string.Empty;
    }

    public Permission(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new BusinessRuleViolationException("Permission.CodeRequired", "Permission code is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("Permission.NameRequired", "Permission name is required.");
        }

        Code = code.Trim();
        Name = name.Trim();
    }

    public string Code { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }
}
