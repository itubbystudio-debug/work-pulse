using Microsoft.AspNetCore.Authorization;

namespace WorkPulse.Api.Authorization;

public sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        var hasPermission = context.User.Claims
            .Where(claim => claim.Type is PermissionClaimTypes.Permission or "permissions")
            .SelectMany(claim => claim.Value.Split(
                [',', ';', ' '],
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Any(permission => string.Equals(
                permission,
                requirement.Permission,
                StringComparison.OrdinalIgnoreCase));

        if (hasPermission)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
