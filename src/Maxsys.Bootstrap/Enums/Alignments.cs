using System.ComponentModel;

namespace Maxsys.Bootstrap;

public enum Alignments : byte
{
    [Description("")]
    None = 0,

    /// <summary>
    /// <c>align-top</c>
    /// </summary>
    [Description("align-top")]
    Top,

    /// <summary>
    /// <c>align-middle</c>
    /// </summary>
    [Description("align-middle")]
    Middle,

    /// <summary>
    /// <c>align-bottom</c>
    /// </summary>
    [Description("align-bottom")]
    Bottom
}