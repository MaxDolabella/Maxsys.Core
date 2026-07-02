using Maxsys.Core.Filtering;
using Maxsys.Core.Sorting;

namespace Maxsys.Core;

public sealed class ListCriteria
{
    public Pagination? Pagination { get; set; } = null;
    public List<SortFilter> Sorts { get; set; } = [];
    public List<ColumnFilter> Filters { get; set; } = [];
    public string? Search { get; set; } = null;
}