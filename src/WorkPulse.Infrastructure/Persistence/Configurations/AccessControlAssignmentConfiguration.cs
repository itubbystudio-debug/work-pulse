using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class AccessControlAssignmentConfiguration : IEntityTypeConfiguration<AccessControlAssignment>
{
    public void Configure(EntityTypeBuilder<AccessControlAssignment> builder)
    {
        builder.ToTable("AccessControlAssignments");
        builder.HasKey(assignment => assignment.Id);
        builder.Ignore(assignment => assignment.DomainEvents);

        builder.Property(assignment => assignment.Action)
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.HasIndex(assignment => new
            {
                assignment.SubjectRecordId,
                assignment.ScreenRecordId,
                assignment.Action,
            })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasOne<SystemAdministrationRecord>()
            .WithMany()
            .HasForeignKey(assignment => assignment.SubjectRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SystemAdministrationRecord>()
            .WithMany()
            .HasForeignKey(assignment => assignment.ScreenRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(assignment => !assignment.IsDeleted);
    }
}
