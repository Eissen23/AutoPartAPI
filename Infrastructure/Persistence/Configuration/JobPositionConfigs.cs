using System;
using System.Collections.Generic;
using System.Text;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration;

public class JobPositionConfigs : IEntityTypeConfiguration<JobPosition>
{
    public void Configure(EntityTypeBuilder<JobPosition> builder)
    {
        builder.ToTable("JobPositions", SchemaNames.Default);

        builder.Property(jobPosition => jobPosition.Title)
            .IsRequired();

        builder.Property(jobPosition => jobPosition.Description)
            .IsRequired(false);

        builder.Property(jobPosition => jobPosition.Salary)
            .IsRequired(false)
            .HasPrecision(18, 2);

        builder.Property(jobPosition => jobPosition.AccessLevel)
           .HasConversion(
                converter => converter.ToString(),
                converter => Enum.Parse<AccessLevel>(converter)
            );

    }
}
