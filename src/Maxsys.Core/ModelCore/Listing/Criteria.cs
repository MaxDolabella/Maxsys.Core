using System.Collections.Generic;
using Maxsys.ModelCore.Sorting;

namespace Maxsys.ModelCore.Listing;

public sealed class Criteria
{
    public Pagination? Pagination { get; set; } = null;
    public HashSet<SortFilter> Sorts { get; set; } = new();

    public static readonly Criteria Empty = new();
}