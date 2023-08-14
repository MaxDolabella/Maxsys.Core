using System.Linq.Expressions;

namespace Maxsys.Core.Sorting.Selectors;

[DependencyInjectionIgnore]
public class NoneColumnSelector<T> : ISortColumnSelector<T>
{
    private static readonly ISortColumnSelector<T> s_instance = new CustomColumnSelector<T>(b => default!);

    public Func<byte?, Expression<Func<T, dynamic>>> ColumnSelector => s_instance.ColumnSelector;
}