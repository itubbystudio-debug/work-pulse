using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class WorkspaceConfiguration : IEntityTypeConfiguration<Workspace>
{
    public void Configure(EntityTypeBuilder<Workspace> builder)
    {
        builder.ToTable("Workspaces");
        builder.HasKey(workspace => workspace.Id);
        builder.Ignore(workspace => workspace.DomainEvents);

        builder.Property(workspace => workspace.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.HasQueryFilter(workspace => !workspace.IsDeleted);
    }
}
