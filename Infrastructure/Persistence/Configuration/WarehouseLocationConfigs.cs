using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Warehouses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class WarehouseLocationConfigs : IEntityTypeConfiguration<WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<WarehouseLocation> builder)
    {
        builder.ToTable("WarehouseLocations", SchemaNames.Default);

        builder
            .Property(wl => wl.ZoneCode)
            .IsRequired();

        builder
            .Property(wl => wl.Aisle)
            .IsRequired();

        builder
            .Property(wl => wl.Shelf)
            .IsRequired();

        builder
            .Property(wl => wl.Bin)
            .IsRequired(false);

        builder
            .Property(wl => wl.IsOverstocked)
            .IsRequired();

        builder
            .HasIndex(wl => wl.ZoneCode)
            .IsUnique()
            .HasDatabaseName("IX_Zonecode");
    }
}
