using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Infrastructure.Cors;

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = [];
}
