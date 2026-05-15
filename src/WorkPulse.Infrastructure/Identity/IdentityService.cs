namespace WorkPulse.Infrastructure.Identity;

public sealed class IdentityService
{
    public Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);
        ArgumentException.ThrowIfNullOrWhiteSpace(role);
        return Task.FromResult(false);
    }
}
