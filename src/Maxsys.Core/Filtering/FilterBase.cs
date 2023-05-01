using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Maxsys.Core.Filtering;

public abstract class FilterBase : IFilter
{
    public SearchFilter? Search { get; set; } = null;
    public ActiveTypes ActiveType { get; set; } = ActiveTypes.OnlyActives;
}

public abstract class FilterBase<TKey> : FilterBase
{
    public KeyList<TKey> IdList { get; set; } = new();
}

public abstract class FilterBase<TKey, T> : FilterBase<TKey>, IFilter<T>
    where T : class
{
    public abstract Expression<Func<T, bool>> ToExpression();
}