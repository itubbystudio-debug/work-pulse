using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");
        builder.HasKey(permission => permission.Id);
        builder.Ignore(permission => permission.DomainEvents);

        builder.Property(permission => permission.Code)
            .HasMaxLength(160)
            .IsRequired();

        builder.Property(permission => permission.Name)
            .HasMaxLength(160)
            .IsRequired();

        builder.HasIndex(permission => permission.Code)
            .IsUnique();

        builder.HasQueryFilter(permission => !permission.IsDeleted);
    }
}
