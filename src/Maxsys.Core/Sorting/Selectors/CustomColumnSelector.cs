using System.Linq.Expressions;

namespace Maxsys.Core.Sorting.Selectors;

[DependencyInjectionIgnore]
public class CustomColumnSelector<T> : ISortColumnSelector<T>
{
    private readonly Expression<Func<T, dynamic>> _expression;

    public CustomColumnSelector(Expression<Func<T, dynamic>> columnSelector)
    {
        _expression = columnSelector;
    }

    public Func<byte?, Expression<Func<T, dynamic>>> ColumnSelector => b => _expression;
}