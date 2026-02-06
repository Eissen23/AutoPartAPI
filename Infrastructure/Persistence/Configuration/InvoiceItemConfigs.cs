using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Invoices;
using Domain.Entities.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class InvoiceItemConfigs : IEntityTypeConfiguration<InvoiceItem>
{
    public void Configure(EntityTypeBuilder<InvoiceItem> builder)
    {
        builder.ToTable("InvoiceItems", SchemaNames.Default);

        builder
            .Property(it => it.InvoiceId)
            .IsRequired();

        builder
            .Property(it => it.ProductId)
            .IsRequired();

        builder
            .Property(it => it.UnitPrice)
            .IsRequired()
            .HasPrecision(18, 2);

        builder .Property(it => it.Quantity)
            .IsRequired();

        builder
            .HasOne(it => it.Invoice)
            .WithOne()
            .HasForeignKey<InvoiceItem>(it => it.InvoiceId);

        builder
            .HasOne(it => it.Product)
            .WithOne()
            .HasForeignKey<InvoiceItem>(it => it.ProductId);
    }
}
