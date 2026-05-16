using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class WorkCalendarConfiguration : IEntityTypeConfiguration<WorkCalendar>
{
    public void Configure(EntityTypeBuilder<WorkCalendar> builder)
    {
        builder.ToTable("WorkCalendars");
        builder.HasKey(calendar => calendar.Id);
        builder.Ignore(calendar => calendar.DomainEvents);

        builder.Property(calendar => calendar.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(calendar => calendar.ShiftCode)
            .HasMaxLength(50);

        builder.Property(calendar => calendar.WorkRuleCode)
            .HasMaxLength(50);

        builder.HasMany(calendar => calendar.ExceptionDates)
            .WithOne()
            .HasForeignKey(exceptionDate => exceptionDate.WorkCalendarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(calendar => calendar.ExceptionDates)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasIndex(calendar => calendar.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(calendar => !calendar.IsDeleted);
    }
}
