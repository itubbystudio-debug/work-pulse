using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Persistence.Configurations;

public sealed class EmployeeGroupConfiguration : IEntityTypeConfiguration<EmployeeGroup>
{
    public void Configure(EntityTypeBuilder<EmployeeGroup> builder)
    {
        builder.ToTable("EmployeeGroups");
        builder.HasKey(employeeGroup => employeeGroup.Id);
        builder.Ignore(employeeGroup => employeeGroup.DomainEvents);

        builder.Property(employeeGroup => employeeGroup.Name)
            .HasMaxLength(120)
            .IsRequired();

        builder.Property(employeeGroup => employeeGroup.Description)
            .HasMaxLength(500);

        builder.Property(employeeGroup => employeeGroup.IsActive)
            .IsRequired();

        builder.HasIndex(employeeGroup => employeeGroup.Name)
            .IsUnique()
            .HasFilter("[IsDeleted] = 0");

        builder.HasQueryFilter(employeeGroup => !employeeGroup.IsDeleted);
    }
}
