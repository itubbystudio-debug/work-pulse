using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkPulse.Application.Features.CompanyProfiles.Commands.UpdateCompanyProfile;
using WorkPulse.Application.Features.CompanyProfiles.Queries.GetCompanyProfile;

namespace WorkPulse.Api.Controllers;

[AllowAnonymous]
public sealed class CompanyProfilesController(IMediator mediator) : BaseApiController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCompanyProfileQuery(), cancellationToken);
        return FromResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(
        UpdateCompanyProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateCompanyProfileCommand(
            request.CompanyName,
            request.TaxId,
            request.BranchName,
            request.Email,
            request.PhoneNumber,
            request.Address,
            request.WebsiteUrl), cancellationToken);

        return FromResult(result);
    }
}

public sealed record UpdateCompanyProfileRequest(
    string CompanyName,
    string TaxId,
    string? BranchName,
    string? Email,
    string? PhoneNumber,
    string? Address,
    string? WebsiteUrl);
