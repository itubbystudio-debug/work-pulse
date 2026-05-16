using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");
        builder.HasKey(department => department.Id);
        builder.Ignore(department => department.DomainEvents);

        builder.Property(department => department.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(department => department.Description)
            .HasMaxLength(500);

        builder.Property(department => department.IsActive)
            .IsRequired();

        builder.HasIndex(department => department.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(department => !department.IsDeleted);
    }
}
