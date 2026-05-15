using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class Workspace : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private Workspace()
    {
        Name = string.Empty;
    }

    public Workspace(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleViolationException("Workspace.NameRequired", "Workspace name is required.");
        }

        Name = name.Trim();
    }

    public string Name { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }
}
