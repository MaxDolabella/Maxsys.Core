using System.ComponentModel;
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
    {
        var methodName = Mode switch
        {
            SearchTermModes.Any => nameof(string.Contains),
            SearchTermModes.StartsWith => nameof(string.StartsWith),
            SearchTermModes.EndsWith => nameof(string.EndsWith),
            _ => throw new InvalidEnumArgumentException(nameof(Mode), (int)Mode, typeof(SearchTermModes)),
        };

        var methodInfo = typeof(string).GetMethod(methodName, [typeof(string)]);

        if (entityFieldArray.Body is NewArrayExpression arrayExpression)
        {
            var filterConstant = Expression.Constant(Term!);

            Expression? memberFilteredExpression = null;

            foreach (var expression in arrayExpression.Expressions)
            {
                if (expression is MemberExpression expressionMember)
                {
                    var memberGetValue = Expression.Lambda<Func<T, string>>(expressionMember, entityFieldArray.Parameters);
                    var filteredMemberValue = Expression.Call(memberGetValue.Body, methodInfo!, filterConstant);

                    memberFilteredExpression = memberFilteredExpression is null
                        ? filteredMemberValue
                        : Expression.Or(memberFilteredExpression, filteredMemberValue);
                }
            }

            return Expression.Lambda<Func<T, bool>>(memberFilteredExpression!, entityFieldArray.Parameters);
        }

        throw new InvalidOperationException("Invalid expression.");
    }
}