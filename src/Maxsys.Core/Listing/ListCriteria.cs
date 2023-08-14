using Maxsys.Core.Sorting;

namespace Maxsys.Core;

public sealed class ListCriteria
{
    public Pagination? Pagination { get; set; } = null;

    public List<SortFilter> Sorts { get; set; } = new();

    public static readonly ListCriteria Empty = new();
}