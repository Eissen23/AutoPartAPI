using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities.FileStorage;

public class FileStorageData : AuditableEntity, IAggregateRoot
{
    public string FileName { get; private set; } = default!;
    public string FileExtension { get; private set; } = default!;

    public string TargetTable { get; private set; } = default!;
    public string TargetId { get; private set; } = default!;

    // the original table for better map (optional)
    public string? RelatedTable { get; private set; }
    public string? RelatedId { get; private set; }

    public FileType FileType { get; private set; }

    public bool IsPublic { get; private set; }
    public FileVisibility FileVisibility { get; private set; }

    // ["Role1", "Role2", ...]
    public string AllowedRoles { get; private set; } = string.Empty;

    public string FilePath { get; private set; } = default!;

    public float FileSize { get; private set; }

    public string? StorageKey { get; private set; }

    public string? CheckSum {  get; private set; }

    public static FileStorageData Create(
        string fileName,
        string fileExtension,
        string targetTable,
        string targetId,
        string? relatedTable,
        string? relatedId,
        FileType fileType,
        bool isPublic,
        FileVisibility fileVisibility,
        string allowedRoles,
        string filePath,
        float fileSize,
        string? storageKey,
        string? checkSum
        )
    {
        var entity = new FileStorageData() {
            FileName = fileName,
            FileExtension = fileExtension,
            TargetTable = targetTable,
            TargetId = targetId,
            RelatedTable = relatedTable,
            RelatedId = relatedId,
            FileType = fileType,
            IsPublic = isPublic,
            FileVisibility = fileVisibility,
            AllowedRoles = allowedRoles,
            FilePath = filePath,
            FileSize = fileSize,
            StorageKey = storageKey,
            CheckSum = checkSum,
        };

        return entity;
    }

    public void Update(
        string? fileName,
        string? fileExtension,
        string? targetTable,
        string? targetId,
        string? relatedTable,
        string? relatedId,
        FileType? fileType,
        bool? isPublic,
        FileVisibility? fileVisibility,
        string? allowedRoles,
        string? filePath,
        float? fileSize,
        string? storageKey,
        string? checkSum
        )
    {
        if (fileName is not null && !FileName.Equals(fileName))
            FileName = fileName;
        if (fileExtension is not null && !FileExtension.Equals(fileExtension))
            FileExtension = fileExtension;
        if (targetTable is not null && !TargetTable.Equals(targetTable))
            TargetTable = targetTable;
        if (targetId is not null && !TargetId.Equals(targetId))
            TargetId = targetId;
        if (RelatedTable?.Equals(relatedTable) is not true)
            RelatedTable = relatedTable;
        if (RelatedId?.Equals(relatedId) is not true)
            RelatedId = relatedId;
        if (fileType.HasValue && FileType != fileType.Value)
            FileType = fileType.Value;
        if (isPublic.HasValue && IsPublic != isPublic.Value)
            IsPublic = isPublic.Value;
        if (fileVisibility.HasValue && FileVisibility != fileVisibility.Value)
            FileVisibility = fileVisibility.Value;
        if (allowedRoles is not null && !AllowedRoles.Equals(allowedRoles))
            AllowedRoles = allowedRoles;
        if (filePath is not null && !FilePath.Equals(filePath))
            FilePath = filePath;
        if (fileSize.HasValue && FileSize != fileSize.Value)
            FileSize = fileSize.Value;
        if (StorageKey?.Equals(storageKey) is not true)
            StorageKey = storageKey;
        if (CheckSum?.Equals(checkSum) is not true)
            CheckSum = checkSum;
    }
}
