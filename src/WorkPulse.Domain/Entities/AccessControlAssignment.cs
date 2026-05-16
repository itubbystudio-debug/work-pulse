using WorkPulse.Domain.Common;
using WorkPulse.Domain.Enums;

namespace WorkPulse.Domain.Entities;

public sealed class AccessControlAssignment : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private AccessControlAssignment()
    {
    }

    private AccessControlAssignment(
        Guid subjectRecordId,
        Guid screenRecordId,
        ActionPermissionType action,
        bool isAllowed)
    {
        SubjectRecordId = subjectRecordId;
        ScreenRecordId = screenRecordId;
        Action = action;
        IsAllowed = isAllowed;
    }

    public Guid SubjectRecordId { get; private set; }

    public Guid ScreenRecordId { get; private set; }

    public ActionPermissionType Action { get; private set; }

    public bool IsAllowed { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public static AccessControlAssignment Create(
        Guid subjectRecordId,
        Guid screenRecordId,
        ActionPermissionType action,
        bool isAllowed)
        => new(subjectRecordId, screenRecordId, action, isAllowed);

    public void SetAllowed(bool isAllowed)
    {
        IsAllowed = isAllowed;
    }
}
