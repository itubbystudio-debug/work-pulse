using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class WorkType : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private WorkType()
    {
        Code = string.Empty;
        Name = string.Empty;
    }

    public WorkType(
        string code,
        string name,
        string? description,
        bool isActive,
        string? policySettingsJson)
    {
        SetDetails(code, name, description, isActive, policySettingsJson);
    }

    public string Code { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }

    public string? PolicySettingsJson { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public void Update(
        string code,
        string name,
        string? description,
        bool isActive,
        string? policySettingsJson)
    {
        SetDetails(code, name, description, isActive, policySettingsJson);
    }

    private void SetDetails(
        string code,
        string name,
        string? description,
        bool isActive,
        string? policySettingsJson)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new BusinessRuleViolationException("WorkType.CodeRequired", "Work type code is required.");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("WorkType.NameRequired", "Work type name is required.");
        }

        Code = code.Trim().ToUpperInvariant();
        Name = name.Trim();
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        IsActive = isActive;
        PolicySettingsJson = string.IsNullOrWhiteSpace(policySettingsJson) ? null : policySettingsJson.Trim();
    }
}
