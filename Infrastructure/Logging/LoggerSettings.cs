using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Logging;

public class LoggerSettings
{
    // I DONT KNOW SHIT ABOUT LOGGING SO THESE ARE JUST PLACEHOLDERS FOR NOW
    public string AppName { get; set; } = "Base.WebAPI";
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = false;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
}
