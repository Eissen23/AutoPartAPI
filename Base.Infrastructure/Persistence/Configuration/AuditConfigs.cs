using System;
using System.Collections.Generic;
using System.Text;
using Base.Infrastructure.Audit;
using Base.Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.Infrastructure.Persistence.Configuration;

public class AuditConfigs : IEntityTypeConfiguration<Trail>
{
    public void Configure(EntityTypeBuilder<Trail> builder)
    {
        builder.ToTable("AuditTrails", SchemaNames.Default);
    }
}
