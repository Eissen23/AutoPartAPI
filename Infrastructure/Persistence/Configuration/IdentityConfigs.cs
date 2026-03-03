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
            .IsRequired(false);

        builder.Property(u => u.DepartmentId)
            .IsRequired(false);

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

public class ApplicationRoleConfiguration 
    : IEntityTypeConfiguration<ApplicationRole>
{
    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        builder.ToTable("Roles");

        builder.Property(r => r.Description)
               .HasMaxLength(500);

        builder.Property(r => r.AccessLevel);

        builder.Property(r => r.IsSystemRole)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(r => r.CreatedAt)
               .IsRequired();

        builder.HasIndex(r => r.Name)
               .IsUnique();
    }
}

public class PermissionConfiguration
    : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("Permissions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.HasIndex(p => p.Name)
               .IsUnique();
    }
}

public class RolePermissionConfiguration
    : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("RolePermissions");

        builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

        builder.HasOne(rp => rp.Role)
               .WithMany()
               .HasForeignKey(rp => rp.RoleId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(rp => rp.Permission)
               .WithMany(p => p.RolePermissions)
               .HasForeignKey(rp => rp.PermissionId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}