using WorkPulse.Domain.Common;
using WorkPulse.Domain.Enums;

namespace WorkPulse.Domain.Entities;

public sealed class SystemAdministrationRecord : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private SystemAdministrationRecord()
    {
        Name = string.Empty;
    }

    private SystemAdministrationRecord(
        SystemAdministrationRecordType type,
        string name,
        string? code,
        string? description,
        Guid? parentId,
        bool isActive)
    {
        Type = type;
        Name = NormalizeRequired(name, "SystemAdministrationRecord.NameRequired", "Name is required.");
        Code = NormalizeOptional(code);
        Description = NormalizeOptional(description);
        ParentId = parentId;
        IsActive = isActive;
    }

    public SystemAdministrationRecordType Type { get; private set; }

    public string Name { get; private set; }

    public string? Code { get; private set; }

    public string? Description { get; private set; }

    public Guid? ParentId { get; private set; }

    public bool IsActive { get; private set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public static SystemAdministrationRecord Create(
        SystemAdministrationRecordType type,
        string name,
        string? code,
        string? description,
        Guid? parentId,
        bool isActive)
        => new(type, name, code, description, parentId, isActive);

    public void Update(string name, string? code, string? description, Guid? parentId, bool isActive)
    {
        Name = NormalizeRequired(name, "SystemAdministrationRecord.NameRequired", "Name is required.");
        Code = NormalizeOptional(code);
        Description = NormalizeOptional(description);
        ParentId = parentId;
        IsActive = isActive;
    }

    private static string NormalizeRequired(string value, string code, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new BusinessRuleViolationException(code, message);
        }

        return value.Trim();
    }

    private static string? NormalizeOptional(string? value)
    {
        var normalized = value?.Trim();
        return string.IsNullOrWhiteSpace(normalized) ? null : normalized;
    }
}
