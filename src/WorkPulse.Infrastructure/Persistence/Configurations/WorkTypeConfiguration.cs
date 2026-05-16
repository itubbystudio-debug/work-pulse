using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class WorkTypeConfiguration : IEntityTypeConfiguration<WorkType>
{
    public void Configure(EntityTypeBuilder<WorkType> builder)
    {
        builder.ToTable("WorkTypes");
        builder.HasKey(workType => workType.Id);
        builder.Ignore(workType => workType.DomainEvents);

        builder.Property(workType => workType.Code)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(workType => workType.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(workType => workType.Description)
            .HasMaxLength(500);

        builder.Property(workType => workType.PolicySettingsJson)
            .HasMaxLength(4000);

        builder.HasIndex(workType => workType.Code)
            .IsUnique();

        builder.HasQueryFilter(workType => !workType.IsDeleted);
    }
}
