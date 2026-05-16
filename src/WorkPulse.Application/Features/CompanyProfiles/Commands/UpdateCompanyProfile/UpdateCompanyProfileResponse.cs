namespace WorkPulse.Application.Features.CompanyProfiles.Commands.UpdateCompanyProfile;

public sealed record UpdateCompanyProfileResponse(
    Guid Id,
    string CompanyName,
    string TaxId,
    string? BranchName,
    string? Email,
    string? PhoneNumber,
    string? Address,
    string? WebsiteUrl);
