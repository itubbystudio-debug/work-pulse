using Microsoft.AspNetCore.Authorization;

namespace WorkPulse.Api.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class RequirePermissionAttribute : AuthorizeAttribute
{
    public RequirePermissionAttribute(string permission)
    {
        Permission = permission;
        Policy = PermissionPolicyNames.ForPermission(permission);
    }

    public string Permission { get; }
}
