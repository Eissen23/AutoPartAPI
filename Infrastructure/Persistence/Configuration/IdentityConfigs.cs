using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class IdentityConfigs : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Configure table name
        builder.ToTable("Users", SchemaNames.Default);

        // Configure foreign keys
        builder.Property(u => u.JobPositionId)
            .IsRequired();

        builder.Property(u => u.DepartmentId)
            .IsRequired();

        // Configure relationships
        builder.HasOne(u => u.JobPosition)
            .WithMany()
            .HasForeignKey(u => u.JobPositionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Users_JobPositions_JobPositionId");

        builder.HasOne(u => u.Department)
            .WithMany()
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("FK_Users_Departments_DepartmentId");
    }
}
