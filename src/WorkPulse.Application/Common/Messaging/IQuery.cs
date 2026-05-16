using MediatR;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Common.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;
