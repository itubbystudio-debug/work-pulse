using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(role => role.Id);
        builder.Ignore(role => role.DomainEvents);

        builder.Property(role => role.Code)
            .HasMaxLength(80)
            .IsRequired();

        builder.Property(role => role.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(role => role.Description)
            .HasMaxLength(500);

        builder.Property(role => role.IsActive)
            .IsRequired();

        builder.HasIndex(role => role.Code)
            .IsUnique();

        builder.HasIndex(role => role.Name)
            .IsUnique();

        builder.HasQueryFilter(role => !role.IsDeleted);
    }
}
