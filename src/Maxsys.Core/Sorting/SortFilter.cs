namespace Maxsys.Core.Sorting;

/// <summary>
/// Filtro para definir ordenação de uma coluna
/// </summary>
public sealed partial class SortFilter
{
    public SortFilter()
    { }

    public SortFilter(string field, SortDirection direction)
    {
        Direction = direction;
        Field = field;
    }

    /// <summary>
    /// é nome do campo que se deseja ordenar
    /// </summary>
    public string? Field { get; set; } = null;

    /// <summary>
    /// é a direção da ordenação.
    /// </summary>
    public SortDirection Direction { get; set; } = SortDirection.Ascending;
}