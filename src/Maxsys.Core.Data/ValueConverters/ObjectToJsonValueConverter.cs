using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Maxsys.Core.Data.ValueConverters;

public sealed class ObjectToJsonValueConverter<TModel> : ValueConverter<TModel?, string?>
{
    public ObjectToJsonValueConverter(JsonSerializerOptions options) : base(
        i => ToDatabase(i, options),
        o => FromDatabase(o, options))
    { }

    public ObjectToJsonValueConverter() : base(
        i => ToDatabase(i, null),
        o => FromDatabase(o, null))
    { }

    private static string? ToDatabase(TModel? value, JsonSerializerOptions? jsonSerializerOptions)
    {
        return value is not null ? JsonSerializer.Serialize(value, jsonSerializerOptions) : default;
    }

    private static TModel? FromDatabase(string? json, JsonSerializerOptions? jsonSerializerOptions)
    {
        return json is not null ? JsonSerializer.Deserialize<TModel>(json, jsonSerializerOptions) : default;
    }
}
