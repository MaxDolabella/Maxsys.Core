namespace Maxsys.Core.Listing;

public sealed class Pagination
{
    public int Index { get; set; } = 0;
    public int Size { get; set; } = 0;

    public bool IsNotEmpty() => Index >= 0 && Size > 0;
}