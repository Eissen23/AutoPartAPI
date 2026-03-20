using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Cors;

public class CorsSettings
{
    public string[] AllowedOrigins { get; set; } = [];
}
