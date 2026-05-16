using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class NavigationMenuConfiguration : IEntityTypeConfiguration<NavigationMenu>
{
    public void Configure(EntityTypeBuilder<NavigationMenu> builder)
    {
        builder.ToTable("NavigationMenus");
        builder.HasKey(menu => menu.Id);
        builder.Ignore(menu => menu.DomainEvents);

        builder.Property(menu => menu.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(menu => menu.Route)
            .HasMaxLength(256);

        builder.Property(menu => menu.Icon)
            .HasMaxLength(80);

        builder.HasMany(menu => menu.Children)
            .WithOne(menu => menu.ParentMenu)
            .HasForeignKey(menu => menu.ParentMenuId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(menu => new { menu.ParentMenuId, menu.DisplayOrder });
        builder.HasQueryFilter(menu => !menu.IsDeleted);
    }
}
