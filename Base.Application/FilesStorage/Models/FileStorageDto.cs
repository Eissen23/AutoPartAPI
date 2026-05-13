using System;
using System.Collections.Generic;
using System.Text;
using Base.Domain.Entities.FileStorage;

namespace Base.Application.FilesStorage.Models;

public class FileStorageDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public string TargetTable { get; set; } = string.Empty;
    public string UserUploadId { get; set; } = string.Empty;
    public string TargetId { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public string FilePath { get; set; } = string.Empty;
    // Internal storage key (relative path) used by storage provider, useful for exports/backups
    public string? StorageKey { get; set; }
    // If file is public this will be a URL (e.g. /uploads/public/{filename})
    public string? PublicUrl { get; set; }
    public string? Checksum { get; set; }
    public float FileSize { get; set; }
    public FileType? FileType { get; set; }
    public FileVisibility FileVisibility { get; set; } = FileVisibility.PRIVATE;
    public string? AllowedRoles { get; set; }
    public string? RelatedTable { get; set; }
    public string? RelatedId { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
}
