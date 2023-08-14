using System.Linq.Expressions;

namespace Maxsys.Core.Filtering;

public class KeyList<TKey> : List<SearchKey<TKey>>// where TKey : struct
{
    #region CTOR

    public KeyList()
    { }

    public KeyList(IEnumerable<TKey> items, SearchKeyModes mode)
    {
        AddRange(items.Select(x => new SearchKey<TKey> { Key = x, Mode = mode }));
    }

    public KeyList(IEnumerable<TKey> itemsToInclude)
        : this(itemsToInclude, SearchKeyModes.Include)
    { }

    public KeyList(IEnumerable<TKey> itemsToInclude, IEnumerable<TKey> itemsToExclude)
    {
        AddRange(itemsToInclude.Select(x => new SearchKey<TKey> { Key = x, Mode = SearchKeyModes.Include }));
        AddRange(itemsToExclude.Select(x => new SearchKey<TKey> { Key = x, Mode = SearchKeyModes.Exclude }));
    }

    #endregion CTOR

    #region Methods

    public bool AnyInclude() => this.Any(k => k.Mode == SearchKeyModes.Include);

    public bool AnyExclude() => this.Any(k => k.Mode == SearchKeyModes.Exclude);

    public void AddItems(IEnumerable<TKey> items, SearchKeyModes mode = SearchKeyModes.Include)
    {
        AddRange(items.Select(x => new SearchKey<TKey> { Key = x, Mode = mode }));
    }

    #endregion Methods

    public static implicit operator KeyList<TKey>(List<TKey> itemsToInclude)
    {
        return new(itemsToInclude);
    }

    public IEnumerable<TKey> Include => this.Where(k => k.Mode == SearchKeyModes.Include).Select(k => k.Key);
    public IEnumerable<TKey> Exclude => this.Where(k => k.Mode == SearchKeyModes.Exclude).Select(k => k.Key);

    public Expression<Func<TEntity, bool>> ToExpression<TEntity>(TKey value)
    {
        return entity => (!AnyInclude() || Include.Contains(value)) && (!AnyExclude() || !Exclude.Contains(value));
    }            
}