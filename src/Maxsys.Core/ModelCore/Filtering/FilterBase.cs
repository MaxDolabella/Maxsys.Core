using System;
using System.Collections.Generic;

namespace Maxsys.ModelCore.Filtering;

public abstract class FilterBase<TKey, TFilter> : IFilter
    where TFilter : class, new()
{
    public static readonly TFilter Empty = new();

    public string? SearchTerm { get; set; } = null;

    public HashSet<TKey> Ids { get; set; } = new();

    public ActiveTypes ActiveType { get; set; } = ActiveTypes.OnlyActives;
}

public abstract class FilterBase<TFilter> : FilterBase<Guid, TFilter>
    where TFilter : class, new()
{ }