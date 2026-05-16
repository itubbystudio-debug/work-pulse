using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("Shifts");
        builder.HasKey(shift => shift.Id);
        builder.Ignore(shift => shift.DomainEvents);

        builder.Property(shift => shift.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(shift => shift.StartTime)
            .HasColumnType("time")
            .IsRequired();

        builder.Property(shift => shift.EndTime)
            .HasColumnType("time")
            .IsRequired();

        builder.HasIndex(shift => shift.Name);
        builder.HasQueryFilter(shift => !shift.IsDeleted);
    }
}
