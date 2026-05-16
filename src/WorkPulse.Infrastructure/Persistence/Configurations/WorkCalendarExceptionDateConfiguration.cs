using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class WorkCalendarExceptionDateConfiguration : IEntityTypeConfiguration<WorkCalendarExceptionDate>
{
    public void Configure(EntityTypeBuilder<WorkCalendarExceptionDate> builder)
    {
        builder.ToTable("WorkCalendarExceptionDates");
        builder.HasKey(exceptionDate => exceptionDate.Id);
        builder.Ignore(exceptionDate => exceptionDate.DomainEvents);

        builder.Property(exceptionDate => exceptionDate.Description)
            .HasMaxLength(200);

        builder.HasIndex(exceptionDate => new { exceptionDate.WorkCalendarId, exceptionDate.Date })
            .IsUnique();
    }
}
