using System;
using System.Collections.Generic;
using System.Text;

namespace Base.Infrastructure.Persistence.Configuration;

internal class SchemaNames
{
    private static string? _defaultSchema;

    public static void Initialize(string schemaName)
    {
        _defaultSchema = schemaName;
    }

    public static string Default => _defaultSchema ?? "QUANLYHOPDONG";

    // Backward compatibility
    public static string QUANLYHOPDONG = "QUANLYHOPDONG"; // Keep for existing migrations
}
