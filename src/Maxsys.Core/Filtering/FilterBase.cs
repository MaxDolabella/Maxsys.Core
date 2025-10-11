using System.Linq.Expressions;
using System.Text.Json.Serialization;
using Maxsys.Core.Helpers;

namespace Maxsys.Core.Filtering;

/// <summary>
/// Fornece uma classe base para implementação de um filtro.
/// </summary>
public abstract class FilterBase : IFilter
{
    [JsonPropertyOrder(-2147483648)]
    public SearchTerm? Search { get; set; } = null;

    [JsonPropertyOrder(-2147483647)]
    public ActiveTypes ActiveType { get; set; } = ActiveTypes.OnlyActives;
}

/// <summary>
/// Fornece uma classe base para implementação de um filtro
/// onde <typeparamref name="TKey"/> é o tipo de chave do objeto que se deseja filtrar.
/// </summary>
public abstract class FilterBase<TKey> : FilterBase, IKeyFilter<TKey>
{
    [JsonPropertyOrder(-2147483646)]
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
    [JsonIgnore]
    public List<Expression<Func<TEntity, bool>>> Expressions { get; } = [];

    /// <summary>
    /// Método responsável por traduzir os filtros em expressions que serão
    /// aplicadas na query no repositório através do método
    /// <see cref="ApplyFilter(ref IQueryable{TEntity})"/>.
    /// </summary>
    public abstract void ConfigureExpressions();

    /// <summary>
    /// Método responsável por chamar <see cref="ConfigureExpressions"/>
    /// e aplicar os filtros ao queryable passado por referência como parâmetro.
    /// </summary>
    /// <param name="queryable">queryable ao qual serão aplicados os filtros.</param>
    public void ApplyFilter(ref IQueryable<TEntity> queryable)
    {
        ConfigureExpressions();

        foreach (var expression in Expressions)
        {
            queryable = queryable.Where(expression);
        }
    }

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

    [Obsolete("Use ConfigureExpressions() method.", true)]
    public virtual void SetExpressions()
    { }
}