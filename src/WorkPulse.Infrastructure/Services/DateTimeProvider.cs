using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Infrastructure.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
