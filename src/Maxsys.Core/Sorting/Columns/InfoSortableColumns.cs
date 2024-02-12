using System.ComponentModel;

namespace Maxsys.Core.Sorting.Columns;

public enum InfoSortableColumns : byte
{
    [SortableProperty("Id")]
    Default = 0,

    [Description("Description")]
    Description = 1,

    [Description("Abbreviation")]
    Abbreviation,

    [Description("CustomState")]
    CustomState
}