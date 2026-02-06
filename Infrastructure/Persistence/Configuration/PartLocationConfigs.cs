using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class PartLocationConfigs : IEntityTypeConfiguration<PartLocation>
{
    public void Configure(EntityTypeBuilder<PartLocation> builder)
    {
        builder.ToTable("PartLocations", SchemaNames.Default);

        builder
            .Property(pl => pl.PartId)
            .IsRequired();

        builder
            .Property(pl => pl.WarehouseLocationId)
            .IsRequired();

        builder
            .Property(pl => pl.QuantityAtLocation)
            .IsRequired();

        builder
            .HasOne(pl => pl.Part)
            .WithOne()
            .HasForeignKey<PartLocation>(pl => pl.PartId);

        builder
            .HasOne(pl => pl.WarehouseLocation)
            .WithOne()
            .HasForeignKey<PartLocation>(pl => pl.WarehouseLocationId);

        builder
            .HasIndex(pl => new { pl.PartId, pl.WarehouseLocationId })
            .HasDatabaseName("IX_WarehouseLocation_Product"); ;
    }
}
