using Maxsys.Core.DTO;
using Maxsys.Core.Sorting.Columns;

namespace Maxsys.Core;

[Sortable<InfoSortableColumns>]
public class InfoDTO<T> : IDTO, IKey<T> where T : notnull
{
    public InfoDTO()
    { }

    public required T Id { get; set; }
    public required string Description { get; set; } = string.Empty;
    public string? Abbreviation { get; set; }
    public object? CustomState { get; set; }
}