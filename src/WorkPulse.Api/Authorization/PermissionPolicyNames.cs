namespace WorkPulse.Api.Authorization;

public static class PermissionPolicyNames
{
    public const string Prefix = "Permission:";

    public static string ForPermission(string permission) => $"{Prefix}{permission}";

    public static bool TryGetPermission(string policyName, out string permission)
    {
        if (policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
        {
            permission = policyName[Prefix.Length..];
            return !string.IsNullOrWhiteSpace(permission);
        }

        permission = string.Empty;
        return false;
    }
}
