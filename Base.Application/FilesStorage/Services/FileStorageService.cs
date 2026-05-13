using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.Common.Interface;
using Base.Application.FilesStorage.Models;
using Base.Domain.Entities.FileStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Common.Exceptions;


namespace Base.Application.FilesStorage.Services;

public interface IStorageService : IScopedService
{
    Task<FileDataResponse?> GetFileDataByPathAsync(string filePath, CancellationToken cancellationToken = default);
    Task<FileStorageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> UploadAsync(
        FileUploadRequest request,
        CancellationToken cancellationToken = default);
    Task<string> UpdateVisibilityAsync(Guid id, FileVisibility visibility, string? allowedRoles, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<string> GeneratePresignedUrlAsync(Guid id, TimeSpan expiresIn, CancellationToken cancellationToken = default);
    Task<Stream> ExportByTargetAsync(string targetTable, string targetId, CancellationToken cancellationToken = default);
    Task<List<FileStorageDto>> GetByTargetAsync(string targetTable, string targetId, CancellationToken cancellationToken = default);
    Task<int> BatchPublishAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
    Task<int> BatchPublishByTargetAsync(string targetTable, string targetId, CancellationToken cancellationToken = default);
}

public class FileStorageService(
    IApplicationDbContext dbContext,
    IDataProtectionProvider dataProtectionProvider,
    IStorageProvider storageProvider,
    ILogger<FileStorageService> logger
    ) : IStorageService
{
    private readonly IApplicationDbContext _dbContext = dbContext;
    private readonly IDataProtector _dataProtector = dataProtectionProvider.CreateProtector("FileStoragePresigned");
    private readonly IStorageProvider _storageProvider = storageProvider;
    private const string StoragePath = "uploads";

    private readonly ILogger<FileStorageService> _logger = logger;



    public Task<int> BatchPublishAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> BatchPublishByTargetAsync(string targetTable, string targetId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Stream> ExportByTargetAsync(string targetTable, string targetId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GeneratePresignedUrlAsync(Guid id, TimeSpan expiresIn, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<FileStorageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var data = await _dbContext.Set<FileStorageData>()
            .AsNoTracking()
            .Where(file => file.Id == id)
            .Select(file => new FileStorageDto
            {
                Id = file.Id,
                Name = file.FileName,
                Extension = file.FileExtension,
                TargetTable = file.TargetTable,
                TargetId = file.TargetId,
                IsPublic = file.IsPublic,
                FilePath = file.FilePath,
                StorageKey = file.StorageKey,
                PublicUrl = file.IsPublic ? file.FilePath : null,
                Checksum = file.CheckSum,
                FileType = file.FileType,
                FileVisibility = file.FileVisibility,
                AllowedRoles = file.AllowedRoles,
                RelatedTable = file.RelatedTable,
                RelatedId = file.RelatedId,
                CreatedAt = file.CreatedOn,
                CreatedBy = file.CreatedBy.ToString()
            })
            .FirstOrDefaultAsync(cancellationToken);

        return data ?? throw new NotFoundException($"File with ID {id} not found.");
    }

    public async Task<List<FileStorageDto>> GetByTargetAsync(string targetTable, string targetId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<FileStorageData>()
            .AsNoTracking()
            .Where(file => file.TargetTable == targetTable && file.TargetId == targetId)
            .Select(file => new FileStorageDto
            {
                Id = file.Id,
                Name = file.FileName,
                Extension = file.FileExtension,
                TargetTable = file.TargetTable,
                TargetId = file.TargetId,
                IsPublic = file.IsPublic,
                FilePath = file.FilePath,
                StorageKey = file.StorageKey,
                PublicUrl = file.IsPublic ? file.FilePath : null,
                Checksum = file.CheckSum,
                FileType = file.FileType,
                FileVisibility = file.FileVisibility,
                AllowedRoles = file.AllowedRoles,
                RelatedTable = file.RelatedTable,
                RelatedId = file.RelatedId,
                CreatedAt = file.CreatedOn,
                CreatedBy = file.CreatedBy.ToString()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<FileDataResponse?> GetFileDataByPathAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return null;

        var cleaned = filePath.Trim();
        var relative = cleaned.TrimStart('/', '\\');

        var candidates = new List<string>();

        if (Path.IsPathRooted(cleaned))
        {
            candidates.Add(Path.GetFullPath(cleaned));
        }

        // Try under wwwroot (public files)
        candidates.Add(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relative)));
        // Try under project root (private files)
        candidates.Add(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), relative)));

        var allowedRoots = new[]
        {
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), StoragePath)),
            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", StoragePath))
        };

        string? found = null;
        foreach (var c in candidates.Distinct())
        {
            if (File.Exists(c))
            {
                var full = Path.GetFullPath(c);
                if (allowedRoots.Any(r => full.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
                {
                    found = full;
                    break;
                }
            }
        }

        if (found == null)
        {
            _logger.LogWarning("File not found on disk: {FilePath}", filePath);
            return null;
        }

        var fileData = await File.ReadAllBytesAsync(found, cancellationToken);
        var fileName = Path.GetFileNameWithoutExtension(found);
        var extension = Path.GetExtension(found);
        var contentType = GetContentType(extension);

        return new FileDataResponse
        {
            Data = fileData,
            FileName = $"{fileName}{extension}",
            ContentType = contentType,
            Extension = extension
        };
    }

    public Task<string> UpdateVisibilityAsync(Guid id, FileVisibility visibility, string? allowedRoles, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> UploadAsync(FileUploadRequest request, CancellationToken cancellationToken = default)
    {
        //if (request.File == null || request.File.Length == 0)
        //    throw new DomainException("File is required");

        //var subfolder = request.IsPublic ? "public" : "private";

        //var saveFolderAbsolute = request.IsPublic
        //    ? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", StoragePath, subfolder)
        //    : Path.Combine(Directory.GetCurrentDirectory(), StoragePath, subfolder);

        //if (!Directory.Exists(saveFolderAbsolute))
        //    Directory.CreateDirectory(saveFolderAbsolute);

        //var fileId = Guid.NewGuid();
        //var originalFileName = Path.GetFileNameWithoutExtension(request.File.FileName);
        //var extension = Path.GetExtension(request.File.FileName);

        //var fileName = $"{fileId}{extension}";
        //var fileAbsolutePath = Path.Combine(saveFolderAbsolute, fileName);

        //using Stream uploadStream = request.File.OpenReadStream();
        //using var ms = new MemoryStream();
        //await uploadStream.CopyToAsync(ms, cancellationToken);
        //ms.Position = 0;

        //string? checksum = null;
        //try
        //{
        //    using var sha = System.Security.Cryptography.SHA256.Create();
        //    ms.Position = 0;
        //    var hash = sha.ComputeHash(ms);
        //    checksum = BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        //    ms.Position = 0;
        //}
        //catch { }

        //var datePath = Path.Combine(DateTime.UtcNow.ToString("yyyy"), DateTime.UtcNow.ToString("MM"), DateTime.UtcNow.ToString("dd"));
        //var subfolder2 = request.IsPublic ? Path.Combine("public", datePath) : Path.Combine("private", datePath);
        //var fileName2 = $"{fileId}{extension}";
        //var relativeStoragePath = Path.Combine(StoragePath, subfolder2, fileName2).Replace('\\', '/');

        //var storageKey = await _storageProvider.SaveAsync(ms, relativeStoragePath, request.IsPublic, cancellationToken);

        //FileType type = request.FileType ?? GetFileType(extension);

        throw new NotImplementedException();
    }

    private static string GetContentType(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".webp" => "image/webp",
            ".svg" => "image/svg+xml",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".zip" => "application/zip",
            ".rar" => "application/x-rar-compressed",
            ".7z" => "application/x-7z-compressed",
            ".mp3" => "audio/mpeg",
            ".mp4" => "video/mp4",
            ".avi" => "video/x-msvideo",
            ".mov" => "video/quicktime",
            _ => "application/octet-stream"
        };
    }

    private static string SanitizeFileName(string fileName)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '_');
        }
        return fileName;
    }

    private static string SanitizePath(string path)
    {
        foreach (var c in Path.GetInvalidPathChars())
        {
            path = path.Replace(c, '_');
        }
        return path;
    }

    private static FileType GetFileType(string extension)
    {
        return extension.ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".webp" or ".svg" => FileType.IMAGE,
            ".pdf" or ".doc" or ".docx" or ".xls" or ".xlsx" or ".ppt" or ".pptx" or
            ".txt" or ".csv" => FileType.DOCUMENT,
            ".json" or ".xml" => FileType.SCRIPTS,
            ".zip" or ".rar" or ".7z" => FileType.BINARY,
            ".mp3" => FileType.AUDIO,
            ".mp4" or ".avi" or ".mov" => FileType.VIDEO,
            _ => FileType.UNKNOWN
        };
    }
}
