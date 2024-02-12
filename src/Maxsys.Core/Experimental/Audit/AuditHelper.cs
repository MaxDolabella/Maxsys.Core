using System.Text.Json;

namespace Maxsys.Experimental.Core.Audit;

public static class AuditHelper
{
    internal static IDictionary<string, JsonElement> GetPropertyDictionary(JsonElement jsonElement, string? parent)
    {
        var dic = new Dictionary<string, JsonElement>();
        foreach (var property in jsonElement.EnumerateObject())
        {
            var propertyKey = property.Name;
            if (parent is not null)
            {
                propertyKey = $"{parent}.{property.Name}";
            }

            if (property.Value.ValueKind == JsonValueKind.Object)
            {
                var others = GetPropertyDictionary(property.Value, propertyKey);
                foreach (var other in others)
                {
                    dic.Add(other.Key, other.Value);
                }
            }
            else
            {
                dic.Add(propertyKey, property.Value);
            }
        }

        return dic;
    }

    public static AuditLog GetAuditLog(object obj1, object obj2)
    {
        // convert JSON to object
        var jsonDoc1 = JsonDocument.Parse(JsonSerializer.Serialize(obj1));
        var jsonDoc2 = JsonDocument.Parse(JsonSerializer.Serialize(obj2));

        // read properties
        var jsonDic1 = GetPropertyDictionary(jsonDoc1.RootElement, null);
        var jsonDic2 = GetPropertyDictionary(jsonDoc2.RootElement, null);

        // find differing properties
        var changes = (from existingProp in jsonDic1
                       from modifiedProp in jsonDic2
                       where modifiedProp.Key.Equals(existingProp.Key)
                       where !modifiedProp.Value.ToString().Equals(existingProp.Value.ToString())
                       select new AuditLogField
                       {
                           Field = existingProp.Key,
                           OldValue = existingProp.Value.ToString(),
                           NewValue = modifiedProp.Value.ToString(),
                       }).ToList();

        var removed = jsonDic1.Where(a => !jsonDic2.Select(x => x.Key).Contains(a.Key))
            .Select(a => new AuditLogField
            {
                Field = a.Key,
                OldValue = a.Value.ToString(),
                NewValue = null,
            }).ToList();

        var inserted = jsonDic2.Where(x => !jsonDic1.Select(a => a.Key).Contains(x.Key))
            .Select(x => new AuditLogField
            {
                Field = x.Key,
                OldValue = null,
                NewValue = x.Value.ToString(),
            }).ToList();

        var logs = changes.Union(removed).Union(inserted).ToArray();

        return new AuditLog { Fields = logs };
    }
}