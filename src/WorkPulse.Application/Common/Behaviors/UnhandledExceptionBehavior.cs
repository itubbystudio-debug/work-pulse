using MediatR;
using Microsoft.Extensions.Logging;

namespace WorkPulse.Application.Common.Behaviors;

public sealed class UnhandledExceptionBehavior<TRequest, TResponse>(
    ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next(cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled exception for request {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}
