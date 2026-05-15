using MediatR;
using WorkPulse.Application.Common.Exceptions;
using WorkPulse.Application.Common.Validation;

namespace WorkPulse.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IValidatableRequest validatableRequest)
        {
            var errors = validatableRequest.Validate();
            if (errors.Count > 0)
            {
                throw new RequestValidationException(errors);
            }
        }

        return await next();
    }
}
