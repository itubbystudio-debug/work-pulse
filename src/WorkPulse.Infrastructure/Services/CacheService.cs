using System.Collections.Concurrent;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Infrastructure.Services;

public sealed class CacheService : ICacheService
{
    private readonly ConcurrentDictionary<string, CacheEntry> _cache = [];

    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (!_cache.TryGetValue(key, out var entry) || entry.ExpiresAt <= DateTimeOffset.UtcNow)
        {
            return Task.FromResult<T?>(default);
        }

        return Task.FromResult((T?)entry.Value);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan duration, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _cache[key] = new CacheEntry(value, DateTimeOffset.UtcNow.Add(duration));
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _cache.TryRemove(key, out _);
        return Task.CompletedTask;
    }

    private sealed record CacheEntry(object? Value, DateTimeOffset ExpiresAt);
}
