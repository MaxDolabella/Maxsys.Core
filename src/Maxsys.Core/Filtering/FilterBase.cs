using System.Linq.Expressions;

namespace Maxsys.Core.Filtering;

/// <summary>
/// Fornece uma classe base para implementa��o de um filtro.
/// </summary>
public abstract class FilterBase : IFilter
{
    public SearchTerm? Search { get; set; } = null;

    public ActiveTypes ActiveType { get; set; } = ActiveTypes.OnlyActives;
}

/// <summary>
/// Fornece uma classe base para implementa��o de um filtro
/// onde <typeparamref name="TKey"/> � o tipo de chave do objeto que se deseja filtrar.
/// </summary>
public abstract class FilterBase<TKey> : FilterBase
{
    public KeyList<TKey> IdList { get; set; } = new();
}

/// <summary>
/// Fornece uma classe base para implementa��o de um filtro
/// onde <typeparamref name="TKey"/> � o tipo de chave da entidade
/// <typeparamref name="TEntity"/> que se deseja filtrar.
/// </summary>
public abstract class FilterBase<TKey, TEntity> : FilterBase<TKey>, IFilter<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Cont�m as expressions que ser�o aplicadas � query.
    /// </summary>
    public List<Expression<Func<TEntity, bool>>> Expressions { get; } = new();

    /// <summary>
    /// Adiciona uma <see cref="Expression"/> para cada filtro �s <see cref="Expressions"/>.
    /// </summary>
    public abstract void SetExpressions();
}