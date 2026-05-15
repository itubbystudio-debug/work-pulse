using MediatR;
using WorkPulse.Application.BackendArchitecture.Models;

namespace WorkPulse.Application.BackendArchitecture.Queries.GetBackendStack;

public sealed record GetBackendStackQuery : IRequest<BackendStackResponseDto>;
