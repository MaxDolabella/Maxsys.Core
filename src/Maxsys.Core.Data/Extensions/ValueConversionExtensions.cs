using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Microsoft.EntityFrameworkCore.Metadata.Builders;
public static class ValueConversionExtensions
{
    //public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> propertyBuilder) where T : class, new()
    //{
    //    ValueConverter<T, string> converter = new ValueConverter<T, string>
    //    (
    //        v => JsonSerializer.Serialize(v),
    //        v => JsonSerializer.Deserialize<T>(v) ?? new T()
    //    );

    //    ValueComparer<T> comparer = new ValueComparer<T>
    //    (
    //        (l, r) => JsonConvert.SerializeObject(l) == JsonConvert.SerializeObject(r),
    //        v => v == null ? 0 : JsonConvert.SerializeObject(v).GetHashCode(),
    //        v => JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(v))
    //    );

    //    propertyBuilder.HasConversion(converter);
    //    propertyBuilder.Metadata.SetValueConverter(converter);
    //    propertyBuilder.Metadata.SetValueComparer(comparer);
    //    propertyBuilder.HasColumnType("jsonb");

    //    return propertyBuilder;
    //}
}