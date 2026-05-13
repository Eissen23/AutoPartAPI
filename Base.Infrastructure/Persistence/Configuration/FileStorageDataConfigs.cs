using System;
using System.Collections.Generic;
using System.Text;
using Base.Domain.Entities.FileStorage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.Infrastructure.Persistence.Configuration;

public class FileStorageDataConfigs : IEntityTypeConfiguration<FileStorageData>
{
    public void Configure(EntityTypeBuilder<FileStorageData> builder)
    {
        builder.Property(f => f.FileName)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(f => f.FileExtension)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(f => f.TargetTable)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(f => f.TargetId)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(f => f.IsPublic)
            .IsRequired();

        builder.Property(f => f.FilePath)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(f => f.FileSize)
            .IsRequired();

        builder.Property(f => f.FileType)
            .HasConversion<int?>();

        builder.Property(f => f.RelatedTable)
            .HasMaxLength(200);

        builder.Property(f => f.RelatedId)
            .HasMaxLength(100);

        // Indexes
        builder.HasIndex(f => f.TargetTable);
        builder.HasIndex(f => f.TargetId);
        builder.HasIndex(f => new { f.TargetTable, f.TargetId });
        builder.HasIndex(f => f.IsPublic);
    }
}
