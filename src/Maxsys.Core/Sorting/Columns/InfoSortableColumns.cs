using System.ComponentModel;

namespace Maxsys.Core.Sorting.Columns;

public enum InfoSortableColumns : byte
{
    [Description("Description")]
    Description = 1,

    [Description("Abbreviation")]
    Abbreviation
}