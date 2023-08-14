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

    /// <summary>
    /// é o valor em byte (item de enum) que representa a coluna que se deseja ordenar
    /// </summary>
    public byte Column { get; set; } = 0;

    /// <summary>
    /// é a direção da ordenação.
    /// </summary>
    public SortDirection Direction { get; set; } = SortDirection.Ascendant;

    /// <summary>
    /// Representa um <see cref="SortFilter"/> padrão com <see cref="Column"/> = 0 e <see cref="Direction"/> = <see cref="SortDirection.Ascendant"/>.
    /// </summary>
    public static readonly SortFilter DEFAULT_SORT = new() { Column = 0, Direction = SortDirection.Ascendant };
}