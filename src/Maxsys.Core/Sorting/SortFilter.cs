namespace Maxsys.Core.Sorting;

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

    public SortFilter(string columnName, SortDirection direction)
    {
        Direction = direction;
        ColumnName = columnName;
    }

    /// <summary>
    /// é o valor em byte (item de enum) que representa a coluna que se deseja ordenar
    /// </summary>
    public byte Column { get; set; } = 0;

    /// <summary>
    /// é nome da coluna que se deseja ordenar
    /// </summary>
    public string? ColumnName { get; set; } = null;

    /// <summary>
    /// é a direção da ordenação.
    /// </summary>
    public SortDirection Direction { get; set; } = SortDirection.Ascendant;
}