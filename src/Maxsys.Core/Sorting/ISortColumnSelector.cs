using System.Linq.Expressions;

namespace Maxsys.Core.Sorting;

public interface ISortColumnSelector<T>
{
    Func<byte?, Expression<Func<T, dynamic>>> ColumnSelector { get; }
}