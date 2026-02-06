using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

internal class CustomerConfigs : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers", SchemaNames.Default);

        builder
            .Property(c => c.Name)
            .IsRequired();

        builder
            .Property(c => c.PhoneNumber)
            .IsRequired(false);

        builder
            .Property(c => c.Email)
            .IsRequired(false);

        builder
            .Property(c => c.CustomerType)
            .HasConversion(
                ct => ct.ToString(),
                s => Enum.Parse<CustomerType>(s)
            )
            .IsRequired();
    }
}
