using System.Diagnostics;

namespace Maxsys.Core.DTO;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class ObjectLink<TNav, TItem>
{
    public required TNav NavigationItem { get; set; }
    public required List<ObjectLinkItem<TItem>> Items { get; set; } = [];

    private string GetDebuggerDisplay()
    {
        return $"[{NavigationItem}, Count={Items.Count}]";
    }
}

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public sealed class ObjectLinkItem<T> : MonitorableDTO
{
    public required T Item { get; set; }

    private string GetDebuggerDisplay()
    {
        return $"[{UpdateStatus}, {Item}]";
    }
}