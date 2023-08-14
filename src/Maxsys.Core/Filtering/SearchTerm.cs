using System.Linq.Expressions;

namespace Maxsys.Core.Filtering;

public class SearchTerm
{
    #region CTOR

    public SearchTerm()
    { }

    public SearchTerm(string term, SearchTermModes mode = SearchTermModes.Any)
    {
        Term = term;
        Mode = mode;
    }

    public SearchTerm(string term, byte column, SearchTermModes mode = SearchTermModes.Any)
        : this(term, mode)
    {
        Column = column;
    }

    #endregion CTOR

    public string? Term { get; set; } = null;
    public SearchTermModes Mode { get; set; } = SearchTermModes.Any;
    public byte Column { get; set; } = 0;

    public Expression<Func<TEntity, bool>> ToExpression<TEntity>(string value)
    {
        return entity => string.IsNullOrWhiteSpace(Term)
            || (Mode == SearchTermModes.Any && value.Contains(Term))
            || (Mode == SearchTermModes.StartsWith && value.StartsWith(Term))
            || (Mode == SearchTermModes.EndsWith && value.EndsWith(Term));
    }
}