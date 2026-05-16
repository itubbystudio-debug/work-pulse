using WorkPulse.Domain;
using WorkPulse.Domain.Common;

namespace WorkPulse.Domain.Entities;

public sealed class CompanyProfile : AggregateRoot, IAuditableEntity
{
    public const string DefaultProfileKey = "default";

    private CompanyProfile()
    {
        ProfileKey = DefaultProfileKey;
        CompanyName = string.Empty;
        TaxId = string.Empty;
    }

    private CompanyProfile(
        string companyName,
        string taxId,
        string? branchName,
        string? email,
        string? phoneNumber,
        string? address,
        string? websiteUrl)
    {
        ProfileKey = DefaultProfileKey;
        CompanyName = string.Empty;
        TaxId = string.Empty;
        Update(companyName, taxId, branchName, email, phoneNumber, address, websiteUrl);
    }

    public string ProfileKey { get; private set; }

    public string CompanyName { get; private set; }

    public string TaxId { get; private set; }

    public string? BranchName { get; private set; }

    public string? Email { get; private set; }

    public string? PhoneNumber { get; private set; }

    public string? Address { get; private set; }

    public string? WebsiteUrl { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTimeOffset? ModifiedAt { get; set; }

    public string? ModifiedBy { get; set; }

    public static CompanyProfile Create(
        string companyName,
        string taxId,
        string? branchName,
        string? email,
        string? phoneNumber,
        string? address,
        string? websiteUrl)
    {
        return new CompanyProfile(companyName, taxId, branchName, email, phoneNumber, address, websiteUrl);
    }

    public void Update(
        string companyName,
        string taxId,
        string? branchName,
        string? email,
        string? phoneNumber,
        string? address,
        string? websiteUrl)
    {
        if (string.IsNullOrWhiteSpace(companyName))
        {
            throw new BusinessRuleViolationException("CompanyProfile.CompanyNameRequired", "Company name is required.");
        }

        if (string.IsNullOrWhiteSpace(taxId))
        {
            throw new BusinessRuleViolationException("CompanyProfile.TaxIdRequired", "Tax ID is required.");
        }

        CompanyName = companyName.Trim();
        TaxId = taxId.Trim();
        BranchName = TrimToNull(branchName);
        Email = TrimToNull(email);
        PhoneNumber = TrimToNull(phoneNumber);
        Address = TrimToNull(address);
        WebsiteUrl = TrimToNull(websiteUrl);
    }

    private static string? TrimToNull(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }
}
