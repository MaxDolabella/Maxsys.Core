namespace Maxsys.Core.Filtering;

public struct SearchKey<TKey> //where TKey : notnull, IComparable<TKey>, IEquatable<TKey>, IEqualityComparer<TKey>
{
    public SearchKey()
        : this(default!)
    { }

    public SearchKey(TKey key)
        : this(key, SearchKeyModes.Include)
    { }

    public SearchKey(TKey key, SearchKeyModes mode)
    {
        Key = key;
        Mode = mode;
    }

    public TKey Key { get; set; }
    public SearchKeyModes Mode { get; set; }

    public override readonly string ToString()
    {
        return $"{Mode} {Key}";
    }
}