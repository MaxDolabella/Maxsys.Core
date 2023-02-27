using System;
using System.Linq.Expressions;

namespace Maxsys.ModelCore.Sorting;

public interface ISortColumnSelector<T>
{
    Func<byte?, Expression<Func<T, dynamic>>> ColumnSelector { get; }
}