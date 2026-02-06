using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class CategoryConfigs : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories", SchemaNames.Default);

        builder.Property(c => c.CategoryCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Name)
            .IsRequired();

        builder.Property(c => c.Description)
            .IsRequired(false);

        builder.Property(c => c.DefaultMarkupPercentage)
            .IsRequired()
            .HasPrecision(18, 2);

        builder
            .Property(c => c.Type)
            .HasConversion(
                c => c.ToString(),
                c => c != null ? Enum.Parse<SystemType>(c) : null
            );

        builder
            .HasIndex(c => c.CategoryCode)
            .IsUnique()
            .HasDatabaseName("IX_Category_Code");
    }
}
