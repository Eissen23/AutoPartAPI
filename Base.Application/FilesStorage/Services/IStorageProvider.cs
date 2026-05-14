using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.FilesStorage.Services;

public interface IStorageProvider
{
    string Name { get; }

    /// <summary>
    /// Save a file stream into storage and return the storage key (relative path)
    /// </summary>
    Task<string> SaveAsync(Stream content, string relativePath, bool isPublic, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete file by storage key
    /// </summary>
    Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Open read stream for the storage key
    /// </summary>
    Task<Stream?> OpenReadAsync(string storageKey, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get absolute file system path for a given storage key (if applicable)
    /// </summary>
    string GetAbsolutePath(string storageKey);
}

