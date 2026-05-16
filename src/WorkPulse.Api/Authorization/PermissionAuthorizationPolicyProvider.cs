using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace WorkPulse.Api.Authorization;

public sealed class PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (!PermissionPolicyNames.TryGetPermission(policyName, out var permission))
        {
            return await base.GetPolicyAsync(policyName);
        }

        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new PermissionAuthorizationRequirement(permission))
            .Build();
    }
}
