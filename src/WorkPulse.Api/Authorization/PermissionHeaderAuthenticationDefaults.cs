namespace WorkPulse.Api.Authorization;

public static class PermissionHeaderAuthenticationDefaults
{
    public const string AuthenticationScheme = "PermissionHeader";
    public const string UserIdHeaderName = "X-User-Id";
    public const string PermissionsHeaderName = "X-Permissions";
}
