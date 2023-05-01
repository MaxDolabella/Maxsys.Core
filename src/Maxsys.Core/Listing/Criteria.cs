using System.Collections.Generic;
using Maxsys.Core.Sorting;

namespace Maxsys.Core.Listing;

/// <summary>
/// Critérios para listagem.
/// </summary>
public sealed class Criteria
{
    public Pagination? Pagination { get; set; } = null;
    public HashSet<SortFilter> Sorts { get; set; } = new();

    public static readonly Criteria Empty = new();
}