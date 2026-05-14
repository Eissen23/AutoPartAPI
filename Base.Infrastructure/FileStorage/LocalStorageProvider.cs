using System;
using System.Collections.Generic;
using System.Text;
using Base.Application.FilesStorage.Services;
using Microsoft.AspNetCore.Hosting;

namespace Base.Infrastructure.FileStorage;

public class LocalStorageProvider(IWebHostEnvironment env) : IStorageProvider
{
    public string Name => "Local";
    private const string StorageRoot = "uploads";
    private readonly string _contentRoot = env.ContentRootPath;

    public async Task<string> SaveAsync(Stream content, string relativePath, bool isPublic, CancellationToken cancellationToken = default)
    {
        var target = isPublic ? Path.Combine("wwwroot", relativePath) : Path.Combine(relativePath);
        var absolute = Path.GetFullPath(Path.Combine(_contentRoot, target));

        var dir = Path.GetDirectoryName(absolute);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir!);

        using var fs = new FileStream(absolute, FileMode.Create, FileAccess.Write, FileShare.None);
        await content.CopyToAsync(fs, cancellationToken);
        await fs.FlushAsync(cancellationToken);

        // Return storage key relative to content root
        var storageKey = Path.Combine(target).Replace('\\', '/');
        if (storageKey.StartsWith("../"))
            storageKey = storageKey[3..];

        return storageKey;
    }

    public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var absolute = GetAbsolutePath(storageKey);
        if (File.Exists(absolute)) File.Delete(absolute);
        return Task.CompletedTask;
    }

    public Task<Stream?> OpenReadAsync(string storageKey, CancellationToken cancellationToken = default)
    {
        var absolute = GetAbsolutePath(storageKey);
        if (!File.Exists(absolute)) return Task.FromResult<Stream?>(null);
        Stream s = new FileStream(absolute, FileMode.Open, FileAccess.Read, FileShare.Read);
        return Task.FromResult<Stream?>(s);
    }

    public string GetAbsolutePath(string storageKey)
    {
        var combined = Path.Combine(_contentRoot, storageKey.TrimStart('/', '\\'));
        return Path.GetFullPath(combined);
    }
}
