using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.CompanyProfiles.Commands.UpdateCompanyProfile;

public sealed class UpdateCompanyProfileCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<UpdateCompanyProfileCommand, Result<UpdateCompanyProfileResponse>>
{
    public async Task<Result<UpdateCompanyProfileResponse>> Handle(
        UpdateCompanyProfileCommand request,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.CompanyProfiles
            .FirstOrDefaultAsync(companyProfile => companyProfile.ProfileKey == CompanyProfile.DefaultProfileKey, cancellationToken);

        if (profile is null)
        {
            profile = CompanyProfile.Create(
                request.CompanyName,
                request.TaxId,
                request.BranchName,
                request.Email,
                request.PhoneNumber,
                request.Address,
                request.WebsiteUrl);

            dbContext.CompanyProfiles.Add(profile);
        }
        else
        {
            profile.Update(
                request.CompanyName,
                request.TaxId,
                request.BranchName,
                request.Email,
                request.PhoneNumber,
                request.Address,
                request.WebsiteUrl);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateCompanyProfileResponse>.Success(new UpdateCompanyProfileResponse(
            profile.Id,
            profile.CompanyName,
            profile.TaxId,
            profile.BranchName,
            profile.Email,
            profile.PhoneNumber,
            profile.Address,
            profile.WebsiteUrl));
    }
}
