using System.ComponentModel;
using System.Linq.Expressions;
using Maxsys.Core.Helpers;

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

    /// <summary>
    /// Obtém uma <see cref="Expression"/> que indica se o <see cref="Term"/> 
    /// e <see cref="Mode"/> correspondem a algum item do array de 
    /// <see cref="string"/> passado através da Expression do parâmetro
    /// <paramref name="entityFieldArray"/>
    /// </summary>
    /// <typeparam name="T">é a entidade a se verificar na Expression</typeparam>
    /// <param name="entityFieldArray"></param>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public Expression<Func<T, bool>> ToExpression<T>(Expression<Func<T, string?[]>> entityFieldArray)
        => ExpressionHelper.SearchTermToExpression(this, entityFieldArray);
}