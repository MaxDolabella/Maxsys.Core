using System.Linq.Expressions;

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
public abstract class FilterBase<TKey> : FilterBase
{
    public KeyList<TKey> IdList { get; set; } = new();
}

/// <summary>
/// Fornece uma classe base para implementação de um filtro
/// onde <typeparamref name="TKey"/> é o tipo de chave da entidade
/// <typeparamref name="TEntity"/> que se deseja filtrar.
/// </summary>
public abstract class FilterBase<TKey, TEntity> : FilterBase<TKey>, IFilter<TEntity>
    where TEntity : class
{
    public List<Expression<Func<TEntity, bool>>> Expressions { get; } = new();

    public abstract void SetExpressions();

    public virtual void AddExpression(Expression<Func<TEntity, bool>> expression)
    {
        Expressions.Add(expression);
    }
}