using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class SystemUserConfiguration : IEntityTypeConfiguration<SystemUser>
{
    public void Configure(EntityTypeBuilder<SystemUser> builder)
    {
        builder.ToTable("SystemUsers");
        builder.HasKey(user => user.Id);
        builder.Ignore(user => user.DomainEvents);

        builder.Property(user => user.IdentityUserId)
            .HasMaxLength(450)
            .IsRequired();

        builder.Property(user => user.UserName)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(user => user.DisplayName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(user => user.Role)
            .HasMaxLength(100)
            .IsRequired();

        builder.HasIndex(user => user.IdentityUserId)
            .IsUnique();

        builder.HasIndex(user => user.UserName)
            .IsUnique();

        builder.HasIndex(user => user.Email)
            .IsUnique();

        builder.HasQueryFilter(user => !user.IsDeleted);
    }
}
