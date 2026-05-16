using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class SystemAdministrationRecordConfiguration : IEntityTypeConfiguration<SystemAdministrationRecord>
{
    public void Configure(EntityTypeBuilder<SystemAdministrationRecord> builder)
    {
        builder.ToTable("SystemAdministrationRecords");
        builder.HasKey(record => record.Id);
        builder.Ignore(record => record.DomainEvents);

        builder.Property(record => record.Type)
            .HasConversion<string>()
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(record => record.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(record => record.Code)
            .HasMaxLength(80);

        builder.Property(record => record.Description)
            .HasMaxLength(500);

        builder.HasIndex(record => new { record.Type, record.Code })
            .IsUnique()
            .HasFilter("[Code] IS NOT NULL AND [IsDeleted] = 0");

        builder.HasOne<SystemAdministrationRecord>()
            .WithMany()
            .HasForeignKey(record => record.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(record => record.ParentId);
        builder.HasQueryFilter(record => !record.IsDeleted);
    }
}
