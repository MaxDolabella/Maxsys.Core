using System;

namespace Maxsys.Core.Filtering;

public class TypedDateRangeFilter<TDateType> : RangeFilter<DateTime?> where TDateType : struct, Enum
{
    public TDateType DateType { get; set; } = default;
}