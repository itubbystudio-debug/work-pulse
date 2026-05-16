using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WorkPulse.Application.Common.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>(
    ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const long LongRunningRequestThresholdMs = 500;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > LongRunningRequestThresholdMs)
        {
            logger.LogWarning(
                "Long-running request {RequestName} took {ElapsedMilliseconds}ms",
                typeof(TRequest).Name,
                stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}
