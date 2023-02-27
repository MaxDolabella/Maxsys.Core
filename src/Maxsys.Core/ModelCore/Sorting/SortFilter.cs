namespace Maxsys.ModelCore.Sorting;

/// <summary>
/// Filtro para definir ordenação de uma coluna
/// </summary>
public sealed class SortFilter
{
    public SortFilter()
    { }

    public SortFilter(byte column, SortDirection direction)
    {
        Direction = direction;
        Column = column;
    }

    /// <summary>
    /// é o nome da coluna que se deseja ordenar
    /// </summary>
    public byte Column { get; set; } = 0;

    /// <summary>
    /// é a direção da ordenação.
    /// </summary>
    public SortDirection Direction { get; set; } = SortDirection.Ascendant;

    public static readonly SortFilter DEFAULT_SORT = new() { Column = 0, Direction = SortDirection.Ascendant };
}