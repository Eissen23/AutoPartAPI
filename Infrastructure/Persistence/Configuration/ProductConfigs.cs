using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class ProductConfigs : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", SchemaNames.Default);

        builder
            .Property(p => p.PartNumber)
            .IsRequired();

        builder
            .Property(p => p.Name)
            .IsRequired();

        builder
            .Property(p => p.Description)
            .IsRequired(false);

        builder
            .Property(p => p.UnitCost)
            .IsRequired()
            .HasPrecision(18, 2);

        builder
            .Property(p => p.RetailPrice)
            .IsRequired(false)
            .HasPrecision(18, 2);

        builder
            .Property(p => p.CategoryId)
            .IsRequired();

        builder
            .HasOne(p => p.Category)
            .WithOne()
            .HasForeignKey<Product>(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(p => p.PartNumber)
            .IsUnique()
            .HasDatabaseName("IX_Product_Number");
    }
}
