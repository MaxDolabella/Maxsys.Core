using System.Linq.Expressions;
using Maxsys.Core.Sorting.Columns;

namespace Maxsys.Core.Sorting.Selectors;

public class InfoByteSortColumnSelector : ISortColumnSelector<InfoDTO<byte>>
{
    public Func<byte?, Expression<Func<InfoDTO<byte>, dynamic>>> ColumnSelector => InfoSortColumnSelector.ColumnSelector<byte>();
}

public class InfoShortSortColumnSelector : ISortColumnSelector<InfoDTO<short>>
{
    public Func<byte?, Expression<Func<InfoDTO<short>, dynamic>>> ColumnSelector => InfoSortColumnSelector.ColumnSelector<short>();
}

public class InfoIntSortColumnSelector : ISortColumnSelector<InfoDTO<int>>
{
    public Func<byte?, Expression<Func<InfoDTO<int>, dynamic>>> ColumnSelector => InfoSortColumnSelector.ColumnSelector<int>();
}

public class InfoLongSortColumnSelector : ISortColumnSelector<InfoDTO<long>>
{
    public Func<byte?, Expression<Func<InfoDTO<long>, dynamic>>> ColumnSelector => InfoSortColumnSelector.ColumnSelector<long>();
}

public class InfoGuidSortColumnSelector : ISortColumnSelector<InfoDTO<Guid>>
{
    public Func<byte?, Expression<Func<InfoDTO<Guid>, dynamic>>> ColumnSelector => InfoSortColumnSelector.ColumnSelector<Guid>();
}

internal static class InfoSortColumnSelector
{
    public static Func<byte?, Expression<Func<InfoDTO<T>, dynamic>>> ColumnSelector<T>() =>
        (columnToSort) => columnToSort.ToByteEnum<InfoSortableColumns>() switch
        {
            InfoSortableColumns.Description => x => x.Description,
            InfoSortableColumns.Abbreviation => x => x.Abbreviation,
            _ => x => x.Id,
        };
}