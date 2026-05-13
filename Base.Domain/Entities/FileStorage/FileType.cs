using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Domain.Entities.FileStorage;

public enum FileType
{
    
    UNKNOWN = 0,
    // PDF, Word, Excel,...
    DOCUMENT = 1,
    // PNG, JPG, SVG,...
    IMAGE = 2,
    // MP4, AVI, MKV,...
    VIDEO = 3,
    // MP3, WAV, AAC,...
    AUDIO = 4,
    // JS, CSS, HTML,...
    SCRIPTS = 5,
    // Other binary files
    BINARY = 6,

}
