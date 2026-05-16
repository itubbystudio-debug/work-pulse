using Microsoft.AspNetCore.Authorization;

namespace WorkPulse.Api.Authorization;

public sealed class PermissionAuthorizationRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}
