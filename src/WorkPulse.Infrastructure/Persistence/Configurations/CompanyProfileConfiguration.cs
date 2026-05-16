using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class CompanyProfileConfiguration : IEntityTypeConfiguration<CompanyProfile>
{
    public void Configure(EntityTypeBuilder<CompanyProfile> builder)
    {
        builder.ToTable("CompanyProfiles");
        builder.HasKey(companyProfile => companyProfile.Id);
        builder.Ignore(companyProfile => companyProfile.DomainEvents);

        builder.Property(companyProfile => companyProfile.ProfileKey)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(companyProfile => companyProfile.ProfileKey)
            .IsUnique();

        builder.Property(companyProfile => companyProfile.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(companyProfile => companyProfile.TaxId)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(companyProfile => companyProfile.BranchName)
            .HasMaxLength(120);

        builder.Property(companyProfile => companyProfile.Email)
            .HasMaxLength(254);

        builder.Property(companyProfile => companyProfile.PhoneNumber)
            .HasMaxLength(50);

        builder.Property(companyProfile => companyProfile.Address)
            .HasMaxLength(500);

        builder.Property(companyProfile => companyProfile.WebsiteUrl)
            .HasMaxLength(300);
    }
}
