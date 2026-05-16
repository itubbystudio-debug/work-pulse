using MediatR;
using Microsoft.Extensions.Logging;

namespace WorkPulse.Application.Common.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling application request {RequestName}", typeof(TRequest).Name);
        var response = await next(cancellationToken);
        logger.LogInformation("Handled application request {RequestName}", typeof(TRequest).Name);
        return response;
    }
}
