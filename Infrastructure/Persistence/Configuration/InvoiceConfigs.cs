using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class InvoiceConfigs : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices", SchemaNames.Default);

        builder
            .Property(i => i.CustomerId)
            .IsRequired();

        builder
            .Property(i => i.SaleDate)
            .IsRequired(false);

        builder
            .Property(i => i.TaxAmount)
            .IsRequired(false)
            .HasPrecision(18, 2);

        builder
            .Property(i => i.TotalAmount)
            .IsRequired(false)
            .HasPrecision(18, 2);

        builder
            .HasOne(i => i.Customer)
            .WithOne()
            .HasForeignKey<Invoice>(i => i.CustomerId)
            .OnDelete(DeleteBehavior.NoAction);
    }

}
