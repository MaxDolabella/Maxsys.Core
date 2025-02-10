using System.Linq.Expressions;
using Maxsys.Core.Helpers;

namespace Maxsys.Core.Filtering;

/// <summary>
/// Fornece uma classe base para implementação de um filtro.
/// </summary>
public abstract class FilterBase : IFilter
{
    public SearchTerm? Search { get; set; } = null;
    public ActiveTypes ActiveType { get; set; } = ActiveTypes.OnlyActives;
}

/// <summary>
/// Fornece uma classe base para implementação de um filtro
/// onde <typeparamref name="TKey"/> é o tipo de chave do objeto que se deseja filtrar.
/// </summary>
public abstract class FilterBase<TKey> : FilterBase, IKeyFilter<TKey>
{
    public KeyList<TKey> IdList { get; set; } = [];
}

/// <summary>
/// Fornece uma classe base para implementação de um filtro
/// onde <typeparamref name="TKey"/> é o tipo de chave da entidade
/// <typeparamref name="TEntity"/> que se deseja filtrar.
/// </summary>
public abstract class FilterBase<TKey, TEntity> : FilterBase<TKey>, IFilter<TEntity>
    where TEntity : class
{
    private bool isApplied = false;
    public List<Expression<Func<TEntity, bool>>> Expressions { get; } = [];

    public void ApplyFilter(IQueryable<TEntity> queryable)
    {
        if (!isApplied)
        {
            ConfigureExpressions();
         
            foreach (var expression in Expressions)
            {
                queryable = queryable.Where(expression);
            }

            isApplied = true;
        }
    }

    [Obsolete("Use ConfigureExpressions() method.", true)]
    public abstract void SetExpressions();
    public abstract void ConfigureExpressions();


    public virtual void AddExpression(Expression<Func<TEntity, bool>> expression)
    {
        Expressions.Add(expression);
    }

    /// <exception cref="InvalidOperationException"></exception>
    public virtual void AddSearchExpression(Expression<Func<TEntity, string?[]>> entityFieldArray)
    {
        if (Search is null)
        {
            throw new InvalidOperationException($"{nameof(Search)} is null. Check {nameof(Search)} nullability before call this method.");
        }

        Expressions.Add(ExpressionHelper.SearchTermToExpression(Search, entityFieldArray));
    }
}