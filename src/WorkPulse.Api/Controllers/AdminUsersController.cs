using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.UserAdministration.Commands.CreateUser;
using WorkPulse.Application.Features.UserAdministration.Commands.SetUserActivation;
using WorkPulse.Application.Features.UserAdministration.Commands.UpdateUser;
using WorkPulse.Application.Features.UserAdministration.Queries.ListUsers;

namespace WorkPulse.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/v{version:int}/admin/users")]
public sealed class AdminUsersController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] string? search,
        [FromQuery] bool? isActive,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new ListUsersQuery(search, isActive), cancellationToken);
        return FromResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new CreateUserCommand(
            request.IdentityUserId,
            request.UserName,
            request.Email,
            request.DisplayName,
            request.Role,
            request.IsActive), cancellationToken);

        return FromResult(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateUserCommand(
            id,
            request.UserName,
            request.Email,
            request.DisplayName,
            request.Role), cancellationToken);

        return FromResult(result);
    }

    [HttpPatch("{id:guid}/activation")]
    public async Task<IActionResult> SetActivation(
        Guid id,
        SetUserActivationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SetUserActivationCommand(id, request.IsActive), cancellationToken);
        return FromResult(result);
    }
}

public sealed record CreateUserRequest(
    string IdentityUserId,
    string UserName,
    string Email,
    string DisplayName,
    string Role,
    bool IsActive = true);

public sealed record UpdateUserRequest(
    string UserName,
    string Email,
    string DisplayName,
    string Role);

public sealed record SetUserActivationRequest(bool IsActive);
