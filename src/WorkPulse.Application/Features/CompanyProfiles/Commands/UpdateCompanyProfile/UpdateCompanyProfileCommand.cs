using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.CompanyProfiles.Commands.UpdateCompanyProfile;

public sealed record UpdateCompanyProfileCommand(
    string CompanyName,
    string TaxId,
    string? BranchName,
    string? Email,
    string? PhoneNumber,
    string? Address,
    string? WebsiteUrl) : ICommand<UpdateCompanyProfileResponse>;
