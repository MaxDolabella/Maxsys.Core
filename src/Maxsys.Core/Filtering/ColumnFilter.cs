using System.Text.Json.Serialization;

namespace Maxsys.Core.Filtering;

public sealed class ColumnFilter
{
    public ColumnFilter()
    { }

    public ColumnFilter(string field, object value) : this(field, value, FilterMatchModes.Contains)
    { }

    public ColumnFilter(string field, object value, FilterMatchModes matchMode)
    {
        Field = field;
        Value = value;
        MatchMode = matchMode;
    }

    public string Field { get; set; } = string.Empty;
    public object? Value { get; set; }
    public FilterMatchModes MatchMode { get; set; } = FilterMatchModes.Contains;

    public override string ToString() => $"{Field} {MatchMode} {Value}";
}

/// <summary>
/// Modos de comparação suportados para filtros de coluna, correspondendo aos matchModes do PrimeNG.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<FilterMatchModes>))]
public enum FilterMatchModes
{
    Contains,       // String contém o valor
    StartsWith,     // String começa com o valor
    EndsWith,       // String termina com o valor
    Equals,         // Igual
    NotEquals,      // Diferente
    In,             // Contido na coleção
    NotIn,          // Não contido na coleção
    Gt,             // Greater than (>)
    Gte,            // Greater than or equal (>=)
    Lt,             // Less than (<)
    Lte,            // Less than or equal (<=)
    Between,        // Entre dois valores [min, max]
    DateIs,         // Data igual
    DateIsNot,      // Data diferente
    DateBefore,     // Data anterior (<)
    DateAfter       // Data posterior (>)
}