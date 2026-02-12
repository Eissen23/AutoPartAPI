using System;
using System.Collections.Generic;
using System.Text;
using Application.Common.Interface;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Serilog.Formatting.Elasticsearch;

namespace Infrastructure.Audit;

internal class AuditTrail(EntityEntry entry, ISerializerService serializerService)
{
    private readonly ISerializerService _serializer =  serializerService;

    public EntityEntry Entry { get; } = entry;

    public Guid UserId { get; set; }
    public string? TableName { get; set; }
    public Dictionary<string, object?> KeyValues { get; } = [];
    public Dictionary<string, object?> OldValues { get; } = [];
    public Dictionary<string, object?> NewValues { get; } = [];
    public List<PropertyEntry> TemporaryProperties { get; } = [];
    public TrailType TrailType { get; set; }
    public List<string> ChangedColumns { get; } = [];
    public bool HasTemporaryProperties => TemporaryProperties.Count > 0;

    public Trail ToAuditTrail() =>
        new()
        {
            UserId = UserId,
            Type = TrailType.ToString(),
            TableName = TableName,
            DateTime = DateTime.UtcNow,
            PrimaryKey = _serializer.Serialize(KeyValues),
            OldValues = OldValues.Count == 0 ? null : _serializer.Serialize(OldValues),
            NewValues = NewValues.Count == 0 ? null : _serializer.Serialize(NewValues),
            AffectedColumns = ChangedColumns.Count == 0 ? null : _serializer.Serialize(ChangedColumns)
        };
}
