using MediatR;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Common.Messaging;

public interface ICommand : IRequest<Result>;

public interface ICommand<TResponse> : IRequest<Result<TResponse>>;
