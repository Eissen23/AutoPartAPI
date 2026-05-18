using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities.FileStorage;

public class FileStorageData : AuditableEntity, IAggregateRoot
{
    public string FileName { get; set; } = default!;
    public string FileExtension { get; set; } = default!;

    public string TargetTable { get; set; } = default!;
    public string TargetId { get; set; } = default!;

    // the original table for better map (optional)
    public string? RelatedTable { get; set; }
    public string? RelatedId { get; set; }

    public FileType FileType { get; set; }

    public bool IsPublic { get; set; }
    public FileVisibility FileVisibility { get; set; }

    // ["Role1", "Role2", ...]
    public string AllowedRoles { get; set; } = string.Empty;

    public string FilePath { get; set; } = default!;

    public float FileSize { get; set; }

    public string? StorageKey { get; set; }

    public string? CheckSum {  get; set; }
}
