using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.CompanyProfiles.Queries.GetCompanyProfile;

public sealed class GetCompanyProfileQueryHandler(IApplicationDbContext dbContext)
    : IRequestHandler<GetCompanyProfileQuery, Result<CompanyProfileDto>>
{
    public async Task<Result<CompanyProfileDto>> Handle(
        GetCompanyProfileQuery request,
        CancellationToken cancellationToken)
    {
        var profile = await dbContext.CompanyProfiles
            .AsNoTracking()
            .Where(companyProfile => companyProfile.ProfileKey == CompanyProfile.DefaultProfileKey)
            .Select(companyProfile => new CompanyProfileDto(
                companyProfile.Id,
                companyProfile.CompanyName,
                companyProfile.TaxId,
                companyProfile.BranchName,
                companyProfile.Email,
                companyProfile.PhoneNumber,
                companyProfile.Address,
                companyProfile.WebsiteUrl))
            .FirstOrDefaultAsync(cancellationToken);

        return profile is null
            ? Result<CompanyProfileDto>.Failure(new Error("CompanyProfile.NotFound", "Company profile has not been configured."))
            : Result<CompanyProfileDto>.Success(profile);
    }
}
