using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class CompanyHolidayConfiguration : IEntityTypeConfiguration<CompanyHoliday>
{
    public void Configure(EntityTypeBuilder<CompanyHoliday> builder)
    {
        builder.ToTable("CompanyHolidays");
        builder.HasKey(holiday => holiday.Id);
        builder.Ignore(holiday => holiday.DomainEvents);

        builder.Property(holiday => holiday.Date)
            .HasColumnType("date")
            .IsRequired();

        builder.Property(holiday => holiday.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasIndex(holiday => holiday.Date)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(holiday => !holiday.IsDeleted);
    }
}
