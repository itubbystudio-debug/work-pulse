using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class PositionConfiguration : IEntityTypeConfiguration<Position>
{
    public void Configure(EntityTypeBuilder<Position> builder)
    {
        builder.ToTable("Positions");
        builder.HasKey(position => position.Id);
        builder.Ignore(position => position.DomainEvents);

        builder.Property(position => position.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(position => position.Description)
            .HasMaxLength(500);

        builder.Property(position => position.IsActive)
            .IsRequired();

        builder.HasOne(position => position.Department)
            .WithMany()
            .HasForeignKey(position => position.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(position => new { position.DepartmentId, position.Name })
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(position => !position.IsDeleted);
    }
}
