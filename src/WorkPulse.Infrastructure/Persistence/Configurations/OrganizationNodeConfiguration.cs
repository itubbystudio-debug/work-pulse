using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class OrganizationNodeConfiguration : IEntityTypeConfiguration<OrganizationNode>
{
    public void Configure(EntityTypeBuilder<OrganizationNode> builder)
    {
        builder.ToTable("OrganizationNodes");
        builder.HasKey(node => node.Id);
        builder.Ignore(node => node.DomainEvents);

        builder.Property(node => node.Name)
            .HasMaxLength(160)
            .IsRequired();

        builder
            .HasOne(node => node.Parent)
            .WithMany()
            .HasForeignKey(node => node.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(node => node.ParentId);
        builder.HasQueryFilter(node => !node.IsDeleted);
    }
}
