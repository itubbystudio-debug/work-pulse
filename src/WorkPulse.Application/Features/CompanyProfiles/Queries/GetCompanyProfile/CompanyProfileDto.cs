namespace WorkPulse.Application.Features.CompanyProfiles.Queries.GetCompanyProfile;

public sealed record CompanyProfileDto(
    Guid Id,
    string CompanyName,
    string TaxId,
    string? BranchName,
    string? Email,
    string? PhoneNumber,
    string? Address,
    string? WebsiteUrl);
