using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Application.FilesStorage.Models;

public class FileDataResponse
{
    public byte[] Data { get; set; } = [];
    public string FileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = "application/octet-stream";
    public string Extension { get; set; } = string.Empty;
}

