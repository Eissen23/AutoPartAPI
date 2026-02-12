using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class DepartmentConfigs : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments", SchemaNames.Default);

        builder.HasKey(department => department.Id);

        builder.Property(department => department.Name)
            .IsRequired();

        builder.Property(department => department.Description)
            .IsRequired();

        builder.HasOne(department => department.Parent)
            .WithOne()
            .HasForeignKey<Department>(department => department.ParentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
