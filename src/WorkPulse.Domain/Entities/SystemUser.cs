using WorkPulse.Domain.Common;
using WorkPulse.Domain;

namespace WorkPulse.Domain.Entities;

public sealed class SystemUser : AggregateRoot, IAuditableEntity, ISoftDelete
{
    private SystemUser()
    {
        IdentityUserId = string.Empty;
        UserName = string.Empty;
        Email = string.Empty;
        DisplayName = string.Empty;
        Role = string.Empty;
    }

    private SystemUser(
        string identityUserId,
        string userName,
        string email,
        string displayName,
        string role,
        bool isActive)
    {
        IdentityUserId = NormalizeRequired(identityUserId, "SystemUser.IdentityUserIdRequired", "Identity user id is required.");
        UserName = NormalizeRequired(userName, "SystemUser.UserNameRequired", "User name is required.");
        Email = NormalizeRequired(email, "SystemUser.EmailRequired", "Email is required.");
        DisplayName = NormalizeRequired(displayName, "SystemUser.DisplayNameRequired", "Display name is required.");
        Role = NormalizeRequired(role, "SystemUser.RoleRequired", "Role is required.");
        IsActive = isActive;
    }

    public string IdentityUserId { get; private set; }

    public string UserName { get; private set; }

    public string Email { get; private set; }

    public string DisplayName { get; private set; }

    public string Role { get; private set; }

    public bool IsActive { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public string? DeletedBy { get; set; }

    public static SystemUser Create(
        string identityUserId,
        string userName,
        string email,
        string displayName,
        string role,
        bool isActive)
    {
        return new SystemUser(identityUserId, userName, email, displayName, role, isActive);
    }

    public void UpdateProfile(string userName, string email, string displayName, string role)
    {
        UserName = NormalizeRequired(userName, "SystemUser.UserNameRequired", "User name is required.");
        Email = NormalizeRequired(email, "SystemUser.EmailRequired", "Email is required.");
        DisplayName = NormalizeRequired(displayName, "SystemUser.DisplayNameRequired", "Display name is required.");
        Role = NormalizeRequired(role, "SystemUser.RoleRequired", "Role is required.");
    }

    public void SetActivation(bool isActive)
    {
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
}
