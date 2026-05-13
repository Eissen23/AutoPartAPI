using System;
using System.Collections.Generic;
using System.Text;
using Base.Domain.Entities.FileStorage;
using Microsoft.AspNetCore.Http;

namespace Base.Application.FilesStorage.Models;

public class FileUploadRequest
{
    public required IFormFile File { get; set; }
    public string TargetTable { get; set; } = string.Empty;
    public string TargetId { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public FileType? FileType{ get; set; }
    public string? RelatedTable { get; set; }
    public string? RelatedId { get; set; }
}
